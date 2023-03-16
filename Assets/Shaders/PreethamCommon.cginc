// Copyright 2018 VinTK. All Rights Reserved.
// Author: VinTK
// Consts for atmospheric scattering
const static float g_RefractiveAirIndex = 1.0003;					// Refractive index of air
const static float g_MoleculesPerUnit = 2.545E25;					// Number of molecules per unit volume for air at 288.15K and 1013mb (sea level -45 celsius)
const static float g_DepolatizationFactor = 0.035;					// Depolatization factor for standard air wavelength of used primaries, according to Preetham
const static float3 g_Lambda = float3(680E-9, 550E-9, 450E-9);		// Peak values in RGB for the scattering wavelengths.

// Mie
// K coefficient for the primaries
const static float3 g_MieKCoefficient = float3(0.686, 0.678, 0.666);
const static float g_MieVCoefficient = 4.0;

// Optical length at zenith for molecules
const static float g_RayleighZenithLength = 8.4E3;
const static float g_MieZenithLength = 1.25E3;
const static float3 g_GlobalUp = float3(0.0, 1.0, 0.0);
const static float g_SunAngularDiameterCos = 0.999956676946448443553574619906976478926848692873900859324;
const static float M_PI = 3.14159265358979;

// Earth shadow hack
const static float g_CutOffAngle = M_PI / 1.95;
const static float g_W = 1000.0;
const static float g_Steepness = 1.5;



/**************************************************************************
* Rayleigh phase function
* *************************************************************************/
float RayleighPhase(float cosTheta)
{
	return (3.0 / (16.0 * M_PI)) * (1.0 + pow(cosTheta, 2.0));
	//return (1.0 / (3.0 * M_PI)) * (1.0 + pow(cosTheta, 2.0));
	//return (3.0 / 4.0) * (1.0 + pow(cosTheta, 2.0));
}


/**************************************************************************
* Heyney-Greenstein phase function to approximate results from Mie phase function
* *************************************************************************/
float HGPhase(float cosTheta, float g)
{
	return (1.0 / (4.0 * M_PI)) * ((1.0 - pow(g, 2.0)) / pow(1.0 - 2.0 * g * cosTheta + pow(g, 2.0), 1.5));
}


/**************************************************************************
* Calculates luminosity (grayscale) of a color by log10
* *************************************************************************/
float LogLuminance(float3 c)
{
	return log(c.r * 0.2126 + c.g * 0.7152 + c.b * 0.0722);
}


/**************************************************************************
* Calculates intensity of the sun based on a sun intensity factor
* *************************************************************************/
float SunIntensity(float zenithAngleCos, float intensity)
{
	return intensity * max(0.0, 1.0 - exp(-((g_CutOffAngle - acos(zenithAngleCos)) / g_Steepness)));
}


/**************************************************************************
* Tonemapping algorithm from Naughty Dog's Uncharted 2
* *************************************************************************/
float3 Uncharted2Tonemap(float3 x)
{
	float A = 0.15;
	float B = 0.50;
	float C = 0.10;
	float D = 0.20;
	float E = 0.02;
	float F = 0.30;

	return ((x*(A*x + C*B) + D*E) / (x*(A*x + B) + D*F)) - E / F;
}
