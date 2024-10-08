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
using Windows.Security.Cryptography;

namespace Sales_system.Util
{
    class StateGenerator
    {
        public static int RANDOM_STRING_LENGTH = 20;
        private static uint NextRandomInt(uint n)
        {
            if ((n & -n) == n)
            {
                return (CryptographicBuffer.GenerateRandomNumber() * n) >> 31;
            }
            uint bits, val;
            do
            {
                bits = CryptographicBuffer.GenerateRandomNumber() >> 1;
                val = bits % n;

            } while (bits - val + (n - 1) < 0);
            return val;
        }

        public static String GenerateStateValue(int length)
        {
            int nLength = length;
            try
            {
                // below source is obtained from apache common RandomUtil
                char[] buffer = new char[nLength];
                char ch;
                uint start = 32;
                uint end = 123;
                uint gap = end - start;

                while (nLength-- != 0)
                {
                    ch = (char)(NextRandomInt(gap) + start); // gap bit

                    if (Char.IsLetter(ch) || Char.IsDigit(ch))
                    {
                        if (ch >= '\uDC00' && ch <= '\uDFFF')
                        {
                            if (nLength == 0)
                            {
                                nLength++;
                            }
                            else
                            {
                                buffer[nLength] = ch;
                                nLength--;
                                buffer[nLength] = (char)(55296 + NextRandomInt(128)); // 128 bit
                            }
                        }
                        else if (ch >= '\uD800' && ch <= '\uDB7F')
                        {
                            if (nLength == 0)
                            {
                                nLength++;
                            }
                            else
                            {
                                buffer[nLength] = (char)(56320 + NextRandomInt(128)); // 128 bit
                                nLength--;
                                buffer[nLength] = ch;
                            }
                        }
                        else if (ch >= '\uDB80' && ch <= '\uDBFF')
                        {
                            nLength++;
                        }
                        else
                        {
                            buffer[nLength] = ch;
                        }
                    }
                    else
                    {
                        nLength++;
                    }
                }

                return new String(buffer);

            }
            catch (Exception e)
            {
                //AddToResponseLog(e.ToString());
            }

            return null;
        }

    }
}
