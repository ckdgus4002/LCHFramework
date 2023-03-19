using System;
using System.Text.RegularExpressions;
using JSchool.Common.Script.Utils;
using UnityEngine;

namespace JSchool.Modules.Common.LCH.Utils
{
    // ************ [공통 모듈 확인됨] ************
    public static class Color
    {
        public static UnityEngine.Color GetColor(string hexadecimalCodeOrNull, UnityEngine.Color result = default)
        {
            if (!string.IsNullOrWhiteSpace(hexadecimalCodeOrNull))
            {
                if (!Regex.IsMatch(hexadecimalCodeOrNull, "#{0,1}([0-9a-fA-F]{2}){3,4}"))
                {
                    FWDebug.LogError($"[GetColor] {hexadecimalCodeOrNull} isn't Match.");
                }
                else
                {
                    if (hexadecimalCodeOrNull[0] == '#') hexadecimalCodeOrNull = hexadecimalCodeOrNull.Substring(1);

                    var r = Mathf.Clamp01(Convert.ToInt32(hexadecimalCodeOrNull.Substring(0, 2), 16) / 255f);
                    var g = Mathf.Clamp01(Convert.ToInt32(hexadecimalCodeOrNull.Substring(2, 2), 16) / 255f);
                    var b = Mathf.Clamp01(Convert.ToInt32(hexadecimalCodeOrNull.Substring(4, 2), 16) / 255f);
                    var a = hexadecimalCodeOrNull.Length == 6 ? 1 : Mathf.Clamp01(Convert.ToInt32(hexadecimalCodeOrNull.Substring(6, 2), 16) / 255f);
                    result = new UnityEngine.Color(r, g, b, a);
                }
            }

            return result;
        }
    }
}