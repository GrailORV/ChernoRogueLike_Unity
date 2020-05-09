﻿using System;

/// <summary>
/// Action型の拡張メソッドを監理するクラス
/// </summary>
public static class ExtensionsAction
{
    /// <summary>
    /// パラメーターを受け取らない Action デリゲートを実行します
    /// </summary>
    /// <param name="action">パラメーターを受け取らない Action デリゲート</param>
    public static void SafeInvoke(this Action action)
    {
        if (action != null)
        {
            action();
        }
    }

    /// <summary>
    /// 1 つのパラメーターを受け取る Action デリゲートを実行します
    /// </summary>
    /// <typeparam name="T">Action デリゲートのパラメーターの型</typeparam>
    /// <param name="action">1 つのパラメーターを受け取る Action デリゲート</param>
    /// <param name="arg">Action デリゲートのパラメーター</param>
    public static void SafeInvoke<T>(this Action<T> action, T arg)
    {
        if (action != null)
        {
            action(arg);
        }
    }

    /// <summary>
    /// 2 つのパラメーターを受け取る Action デリゲートを実行します
    /// </summary>
    /// <typeparam name="T1">Action デリゲートの第 1 パラメーターの型</typeparam>
    /// <typeparam name="T2">Action デリゲートの第 2 パラメーターの型</typeparam>
    /// <param name="action">2 つのパラメーターを受け取る Action デリゲート</param>
    /// <param name="arg1">Action デリゲートの第 1 パラメーター</param>
    /// <param name="arg2">Action デリゲートの第 2 パラメーター</param>
    public static void SafeInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
    {
        if (action != null)
        {
            action(arg1, arg2);
        }
    }

    /// <summary>
    /// 3 つのパラメーターを受け取る Action デリゲートを実行します
    /// </summary>
    /// <typeparam name="T1">Action デリゲートの第 1 パラメーターの型</typeparam>
    /// <typeparam name="T2">Action デリゲートの第 2 パラメーターの型</typeparam>
    /// <typeparam name="T3">Action デリゲートの第 3 パラメーターの型</typeparam>
    /// <param name="action">3 つのパラメーターを受け取る Action デリゲート</param>
    /// <param name="arg1">Action デリゲートの第 1 パラメーター</param>
    /// <param name="arg2">Action デリゲートの第 2 パラメーター</param>
    /// <param name="arg3">Action デリゲートの第 3 パラメーター</param>
    public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> action, T1 arg1, T2 arg2, T3 arg3)
    {
        if (action != null)
        {
            action(arg1, arg2, arg3);
        }
    }

    /// <summary>
    /// 4 つのパラメーターを受け取る Action デリゲートを実行します
    /// </summary>
    /// <typeparam name="T1">Action デリゲートの第 1 パラメーターの型</typeparam>
    /// <typeparam name="T2">Action デリゲートの第 2 パラメーターの型</typeparam>
    /// <typeparam name="T3">Action デリゲートの第 3 パラメーターの型</typeparam>
    /// <typeparam name="T4">Action デリゲートの第 4 パラメーターの型</typeparam>
    /// <param name="action">4 つのパラメーターを受け取る Action デリゲート</param>
    /// <param name="arg1">Action デリゲートの第 1 パラメーター</param>
    /// <param name="arg2">Action デリゲートの第 2 パラメーター</param>
    /// <param name="arg3">Action デリゲートの第 3 パラメーター</param>
    /// <param name="arg4">Action デリゲートの第 4 パラメーター</param>
    public static void SafeInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> action, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
    {
        if (action != null)
        {
            action(arg1, arg2, arg3, arg4);
        }
    }
}
