using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Sdl.LanguagePlatform.Core;

namespace LetsMT.MTProvider
{
    public class CMtProfileCollection
    {
        //Contains all unique language directions
        private List<CMtProfile> m_profileList;

        //Contains references to all available systems inside language directions
        private List<CMtSystem> m_systemList;

        public CMtProfileCollection(LetsMTAPI.MTSystem[] mtSystems)
        {
            //Empty lists initialized
            m_profileList = new List<CMtProfile>();
            m_systemList = new List<CMtSystem>();

            //Fill the lists with data from web service
            foreach (LetsMTAPI.MTSystem system in mtSystems)
            {
                string strProfileId = CMtProfile.GenerateProfileId(system.SourceLanguage.Code, system.TargetLanguage.Code);

                //Reference to profile which has to be filled with system
                CMtProfile refProfile = null;

                foreach (CMtProfile existingProfile in m_profileList)
                {
                    //We already have a profile
                    if(existingProfile.IsProfile(strProfileId))
                    {
                        //Set the reference to an existing profile
                        refProfile = existingProfile;
                        break;
                    }
                }

                //No profile, create new and fill the fields
                if (refProfile == null)
                {
                    string strFriendlyName;
                    strFriendlyName = string.Format("{0} - {1}", system.SourceLanguage.Name.Text, system.TargetLanguage.Name.Text);
                    
                    //Set the reference to new profile
                    refProfile = new CMtProfile(system.SourceLanguage.Code, system.SourceLanguage.Name.Text, system.TargetLanguage.Code, system.TargetLanguage.Name.Text);

                    m_profileList.Add(refProfile);
                }

                //Add a system to profile through profile reference
                string strSysId = system.ID;
                string strSysFriendlyName = system.Title.Text;
                string strSysDescription = system.Description.Text;
               // string strSysDescription = system.Metadata[0].
                string strSysOnlineStatus = "unknown";
                strSysDescription += "";
                //Fill description field
                foreach (LetsMTAPI.ObjectProperty meta in system.Metadata)
                {
                    if (meta.Key == "description")
                    {
                        strSysDescription += "Description: " + meta.Value + "\n";
                    }
                    if ( meta.Key.StartsWith("score") && !(meta.Key.Contains("-c")))
                    {
                        try
                        {
                            double x;
                            if (double.TryParse(meta.Value, NumberStyles.Any, CultureInfo.GetCultureInfoByIetfLanguageTag("EN-US"), out x))
                               {
                                   double score = x;
                                   score = Math.Round(score, 4);
                                   if (meta.Key.Contains("bleu")) { score = score * 100; }
                                   strSysDescription += meta.Key.Replace("score-", "").ToUpper() + ":" + score.ToString() + ", ";
                               }
                            else { strSysDescription += meta.Key.Replace("score-", "").ToUpper() + ":" + meta.Value + ", "; }
                        }
                        catch {continue;}
             
                    }

                }
                char[] charsToTrim = {',', ' '};
                strSysDescription = strSysDescription.TrimEnd(charsToTrim);

                System.Collections.IEnumerator metaEnum = system.Metadata.GetEnumerator();
                while(metaEnum.MoveNext())
                {
                    Nullable<LetsMTAPI.ObjectProperty> nullableProp = metaEnum.Current as Nullable<LetsMTAPI.ObjectProperty>;
                    LetsMTAPI.ObjectProperty prop = nullableProp.Value;  //TODO: check if not null after cast
                    if(prop.Key == "status")
                    {
                        switch (prop.Value)
                        {
                            case "running":
                                strSysOnlineStatus = "Running";
                                break;
                            case "queuingtransl":
                                strSysOnlineStatus = "Queuing";
                                break;
                            case "notstarted":
                                strSysOnlineStatus = "Not Started";
                                break;
                            case "nottrained":
                                strSysOnlineStatus = "Not Trained";
                                break;
                            case "error":
                                strSysOnlineStatus = "Not Trained";
                                break;
                            case "training":
                                strSysOnlineStatus = "Training";
                                break;
                            case "standby":
                                strSysOnlineStatus = "Standby";
                                break;
                            default:
                                strSysOnlineStatus = prop.Value;
                                break;
                        }
                        //strSysOnlineStatus = prop.Value;
                        break;
                    }
                }

                CMtSystem refSystem = refProfile.AddSystem(strSysId, strSysFriendlyName, strSysDescription, strSysOnlineStatus);

                m_systemList.Add(refSystem);
            }
        }

        //Get profiles as listbox items
        public List<ListItem> GetProfileListItems(bool bFiltered = false)
        {
            List<ListItem> profiles = new List<ListItem>();

            foreach (CMtProfile profile in m_profileList)
            {
                if(!bFiltered || profile.HasOnlineSystems())
                    profiles.Add(profile.GetListItem());
            }

            return profiles;
        }

        public List<CMtProfile> GetProfileList(bool bFiltered = false)
        {
            List<CMtProfile> profiles = new List<CMtProfile>();

            foreach (CMtProfile profile in m_profileList)
            {
                if (!bFiltered || profile.HasOnlineSystems())
                    profiles.Add(profile);
            }

            return profiles;
        }

        //Get systems of the required profile as listbox items
        public List<ListItem> GetProfileSystemList(string strProfileId, bool bFiltered = false)
        {
            List<ListItem> systems = new List<ListItem>();

            foreach (CMtProfile profile in m_profileList)
            {
                if (profile.IsProfile(strProfileId))
                {
                    systems = profile.GetSystemList(bFiltered);
                    break;
                }
            }

            return systems;
        }

        //Get system reference by id, used for obtaining systems data
        public CMtSystem GetSystemById(string strId)
        {
            foreach (CMtSystem system in m_systemList)
            {
                if (system.IsSystem(strId))
                {
                    return system;
                }
            }

            return null;
        }

        public bool HasProfile(string strProfile)
        {
            foreach (CMtProfile profile in m_profileList)
            {
                if (profile.IsProfile(strProfile))
                    return true;
            }

            return false;
        }

        public bool HasProfile(LanguagePair lpProfile)
        {
            foreach (CMtProfile profile in m_profileList)
            {
                if (profile.IsProfile(lpProfile))
                    return true;
            }

            return false;
        }

        public string GetActiveSystemForProfile(string strProfile)
        {
            foreach (CMtProfile profile in m_profileList)
            {
                if (profile.IsProfile(strProfile))
                {
                    return profile.GetDefaultSystem();
                }
            }

            return "";
        }

        public void SetActiveSystemForProfile(string strProfile, string strState)
        {
            foreach (CMtProfile profile in m_profileList)
            {
                if (profile.IsProfile(strProfile))
                {
                    profile.SetDefaultSystem(strState);
                    break;
                }
            }
        }

        public string GetActiveSystemForProfile(LanguagePair lpProfile)
        {
            foreach (CMtProfile profile in m_profileList)
            {
                if (profile.IsProfile(lpProfile))
                {
                    return profile.GetDefaultSystem();
                }
            }

            return "";
        }

        public string SerializeState()
        {
            List<ProfileInfo> profileInfos = new List<ProfileInfo>();

            foreach (CMtProfile profile in m_profileList)
            {
                string strDefaultSystem = profile.GetDefaultSystem();
                Dictionary<string, string> defaultTermCorpora = profile.m_defaultTermCorpora;

                if (!string.IsNullOrEmpty(strDefaultSystem) || defaultTermCorpora.Count > 0)
                {
                    SerializableDictionary<string, string> serializableDefaultTermCorpora = defaultTermCorpora as SerializableDictionary<string, string>;
                    profileInfos.Add(new ProfileInfo { ProfileId = profile.GetProfileId(), DefaultSystemId = strDefaultSystem, DefaultTermCorpora = serializableDefaultTermCorpora });
                }
            }

            return profileInfos.SerializeObject();
        }

        public void DeserializeState(string state)
        {
            if (string.IsNullOrEmpty(state))
                return;
            List<ProfileInfo> profileInfos;
            try
            {
                profileInfos = Utilities.DeserializeObject<List<ProfileInfo>>(state);
            }
            catch
            {
                return;
            }
            foreach (ProfileInfo profileInfo in profileInfos)
            {
                if(string.IsNullOrEmpty(profileInfo.ProfileId) ||
                    (string.IsNullOrEmpty(profileInfo.DefaultSystemId) && profileInfo.DefaultTermCorpora.Count == 0))
                {
                    continue;
                }

                foreach (CMtProfile profile in m_profileList)
                {
                    if (profile.IsProfile(profileInfo.ProfileId))
                    {
                        profile.SetDefaultSystem(profileInfo.DefaultSystemId);
                        if (profileInfo.DefaultTermCorpora != null)
                        {
                            foreach (KeyValuePair<string, string> defaultCorpora in profileInfo.DefaultTermCorpora)
                            {
                                profile.SetDefaultTermCorpora(defaultCorpora.Key, defaultCorpora.Value); //TODO: check the running time of this. Currently each passed systemId is again checked against all available systems
                            }
                        }
                        break;
                    }
                }
            }
        }
    }

    #region serialization helper class
    [Serializable]
    public class ProfileInfo
    {
        public string ProfileId { get; set; }
        public string DefaultSystemId { get; set; }
        public SerializableDictionary<string, string> DefaultTermCorpora { get; set; }
    }
    #endregion
}
