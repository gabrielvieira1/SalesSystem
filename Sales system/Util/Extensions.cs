/*
  * Copyright (c) 2000-2018 Samsung Electronics Co., Ltd All Rights Reserved
  *
  * PROPRIETARY/CONFIDENTIAL
  *
  * This software is the confidential and proprietary information of
  * SAMSUNG ELECTRONICS ("Confidential Information").
  * You shall not disclose such Confidential Information and shall
  * use it only in accordance with the terms of the license agreement
  * you entered into with SAMSUNG ELECTRONICS.
  * SAMSUNG make no representations or warranties about the suitability
  * of the software, either express or implied, including but not
  * limited to the implied warranties of merchantability, fitness for
  * a particular purpose, or non-infringement.
  * SAMSUNG shall not be liable for any damages suffered by licensee as
  * a result of using, modifying or distributing this software or its derivatives.
*/
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;

namespace Sales_system.Util
{
    public static class Extensions
    {
        public static string ObjectDictionaryToString(this ValueSet objDict)
        {
            int len = objDict.Count;
            int i = 0;
            string str = "";
            foreach (KeyValuePair<string, object> keyValuePair in objDict)
            {
                if (i == len - 1)
                    str += string.Format("[{0}] = {1}", keyValuePair.Key.ToString(), keyValuePair.Value == null ? "null" : keyValuePair.Value.ToString());
                else
                    str += string.Format("[{0}] = {1}, ", keyValuePair.Key.ToString(), keyValuePair.Value == null ? "null" : keyValuePair.Value.ToString());
            }
            return str;
        }

        public static ValueSet ObjectDictionaryToStringDictionary(this ValueSet objDict)
        {
            ValueSet strDict = new ValueSet();
            foreach (KeyValuePair<string, object> keyValuePair in objDict)
            {
                strDict.Add(keyValuePair.Key, keyValuePair.Value == null ? null : keyValuePair.Value.ToString());
            }
            return strDict;
        }

        public static ValueSet ToStringDictionary(this ValueSet objDict)
        {
            ValueSet strDict = new ValueSet();
            foreach (KeyValuePair<string, object> keyValuePair in objDict)
            {
                strDict.Add(keyValuePair.Key, keyValuePair.Value == null ? null : keyValuePair.Value.ToString());
            }
            return strDict;
        }
        public static String ToStringValue(this ValueSet objDict)
        {
            return objDict.StringDictionaryToString();
        }

        //public static Dictionary<object, object> ToDictionary(this IDictionary dict)
        //{
        //    return dict.Keys.Cast<object>().ToDictionary(k => k, k => dict[k]);
        //}

        public static String StringDictionaryToString(this ValueSet objDict)
        {
            int len = objDict.Count;
            int i = 0;
            String str = "";
            foreach (KeyValuePair<string, object> keyValuePair in objDict)
            {
                if (i == len - 1)
                    str += String.Format("[{0}] = {1}", keyValuePair.Key, keyValuePair.Value == null ? "null" : keyValuePair.Value);
                else
                    str += String.Format("[{0}] = {1}, ", keyValuePair.Key, keyValuePair.Value == null ? "null" : keyValuePair.Value);
                i++;
            }
            return str;
        }
        public static String GetFirstValueByName(this Windows.Foundation.WwwFormUrlDecoder decoder, String keyName, String defaultValue)
        {
            String value = defaultValue;
            try
            {
                value = decoder.GetFirstValueByName(keyName);
            }
            catch (Exception)
            {
                return value;
            }
            return value;
        }
        public static Object GetValue(this Windows.Storage.ApplicationDataCompositeValue bundle, String key, Object defaultValue)
        {
            if (bundle.ContainsKey(key))
            {
                return bundle[key];
            }
            return defaultValue;
        }
        public static Object GetValue(this ValueSet bundle, String key, Object defaultValue)
        {
            if (bundle.ContainsKey(key))
            {
                return bundle[key];
            }
            return defaultValue;
        }
    }
}
