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
using Models;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;
using Windows.ApplicationModel;
using Windows.Security.ExchangeActiveSyncProvisioning;
using Windows.System.Profile;
using Windows.Management.Deployment;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation.Collections;
using System.Collections;
using Windows.System;
using System.Threading.Tasks;
using SamsungAccountLibrary;
using SamsungAccountLibrary.SAResponse;

namespace Sales_system
{
    public sealed class Utils
    {

        public static bool IsSuccessed(string response, String responseState)
        {
            if (response.Equals(ResultType.TRUE) && responseState.Equals(MetaDataManager.GetInstance().GetState()))
            {
                return true;
            }
            return false;
        }

        public static bool IsSuccessed(string response)
        {
            if (response.Equals(ResultType.TRUE))
            {
                return true;
            }
            return false;
        }

        public static String GetDevicePhysicalAddress()
        {

            // get device unique id
            IBuffer idBuffer = SystemIdentification.GetSystemIdForPublisher().Id;
            string id = CryptographicBuffer.EncodeToBase64String(idBuffer); // 44 bytes

            // encryption
            IBuffer buffer = CryptographicBuffer.ConvertStringToBinary(id, BinaryStringEncoding.Utf8);
            HashAlgorithmProvider hash_sha1 = HashAlgorithmProvider.OpenAlgorithm(HashAlgorithmNames.Sha1);
            IBuffer hashed_buffer = hash_sha1.HashData(buffer); // 160 bits

            string hashedString = CryptographicBuffer.EncodeToHexString(hashed_buffer); // 40 bytes

            // generate device id (DEVICE_TYPE_VALUE:05 if PC)
            //device_id = DEVICE_TYPE_VALUE + hashedString;

            //return device_id; // 42bytes

            return hashedString; // 40 bytes
        }

        public static Boolean IsValidServerUrl(String value)
        {

            String[] authServerUrl = {"us-auth2.samsungosp.com", "eu-auth2.samsungosp.com", "cn-auth2.samsungosp.com.cn" ,
                "apigateway-sas-eucentral1.samsungospdev.com" , "stg-us-auth2.samsungosp.com","cn-api.samsungosp.com","cn-auth.samsungosp.com",
            "api.samsungosp.com","auth.samsungosp.com"};


            if (Array.IndexOf(authServerUrl, value) < 0)
            {
                return false;
            }

            else
            {
                return true;
            }

        }
    }


    public static class Extensions
    {
        public static string ObjectDictionaryToString(this Dictionary<object, object> objDict)
        {
            int len = objDict.Count;
            int i = 0;
            string str = "";
            foreach (KeyValuePair<object, object> keyValuePair in objDict)
            {
                if (i == len - 1)
                    str += string.Format("[{0}] = {1}", keyValuePair.Key.ToString(), keyValuePair.Value == null ? "null" : keyValuePair.Value.ToString());
                else
                    str += string.Format("[{0}] = {1}, ", keyValuePair.Key.ToString(), keyValuePair.Value == null ? "null" : keyValuePair.Value.ToString());
            }
            return str;
        }

        public static Dictionary<String, String> ObjectDictionaryToStringDictionary(this Dictionary<object, object> objDict)
        {
            Dictionary<String, String> strDict = new Dictionary<String, String>();
            foreach (KeyValuePair<object, object> keyValuePair in objDict)
            {
                strDict.Add(keyValuePair.Key.ToString(), keyValuePair.Value == null ? null : keyValuePair.Value.ToString());
            }
            return strDict;
        }

        public static Dictionary<String, String> ToStringDictionary(this ValueSet objDict)
        {
            return objDict.ToDictionary(x => x.Key, x => x.Value.ToString());
        }
        public static String ToStringValue(this ValueSet objDict)
        {
            return objDict.ToDictionary(x => x.Key, x => (x.Value == null) ? "" : x.Value.ToString()).StringDictionaryToString();
        }

        public static Dictionary<object, object> ToDictionary(this IDictionary dict)
        {
            return dict.Keys.Cast<object>().ToDictionary(k => k, k => dict[k]);
        }

        public static String StringDictionaryToString(this Dictionary<String, String> objDict)
        {
            int len = objDict.Count;
            int i = 0;
            String str = "";
            foreach (KeyValuePair<String, String> keyValuePair in objDict)
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

        public static object GetValue(this ValueSet bundle, String key, Object defaultValue)
        {
            object output = null;
            if (bundle.ContainsKey(key))
            {
                bundle.TryGetValue(key, out output);
                return output;
            }
            return defaultValue;
        }

    }
}
