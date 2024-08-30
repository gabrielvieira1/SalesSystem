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
namespace Sales_system
{
  [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
  [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
  public partial class AuthorizeTokenResultVO
  {

    private string authorizeCodeField;

    private string authorizeDescField;

    private string authenticateUserIDField;

    private uint remainExpireTimeSecField;

    /// <remarks/>
    public string authorizeCode
    {
      get
      {
        return this.authorizeCodeField;
      }
      set
      {
        this.authorizeCodeField = value;
      }
    }

    /// <remarks/>
    public string authorizeDesc
    {
      get
      {
        return this.authorizeDescField;
      }
      set
      {
        this.authorizeDescField = value;
      }
    }

    /// <remarks/>
    public string authenticateUserID
    {
      get
      {
        return this.authenticateUserIDField;
      }
      set
      {
        this.authenticateUserIDField = value;
      }
    }

    /// <remarks/>
    public uint remainExpireTimeSec
    {
      get
      {
        return this.remainExpireTimeSecField;
      }
      set
      {
        this.remainExpireTimeSecField = value;
      }
    }
  }
}

