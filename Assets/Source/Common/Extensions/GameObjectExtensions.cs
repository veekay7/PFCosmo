// Copyright 2018 Nanyang Technological University. All Rights Reserved.
// Authors: VinTK
using System;

namespace UnityEngine.Extensions
{
    public static class GameObjectExtensions
    {
        public static GameRules GetGameRules(this GameObject gameObject)
        {
            GameRules instance = GameRules.instance;
            return instance;
        }


        public static T GetGameRulesAs<T>(this GameObject gameObject) where T : GameRules
        {
            T inst = (T)GameRules.instance;
            if (inst == null)
                throw new InvalidCastException("instance");

            return inst;
        }
    }
}
