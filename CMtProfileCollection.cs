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
                string strProfileId = string.Format("{0} - {1}", system.SourceLanguage.Code, system.TargetLanguage.Code);

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
                    strFriendlyName = string.Format("{0} - {1}", system.SourceLanguage.Name.Text, system.TargetLanguage.Name.Text);  //TODO: check if this shouldn't be *.Name.Language instead
                    
                    //Set the reference to new profile
                    refProfile = new CMtProfile(strProfileId, strFriendlyName);

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
        public List<ListItem> GetProfileList(bool bFiltered = false)
        {
            List<ListItem> profiles = new List<ListItem>();

            foreach (CMtProfile profile in m_profileList)
            {
                if(!bFiltered || profile.HasOnlineSystems())
                    profiles.Add(profile.GetListItem());
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
            StringBuilder sb = new StringBuilder();

            foreach (CMtProfile profile in m_profileList)
            {
                string strDefaultSystem = profile.GetDefaultSystem();
                if (!string.IsNullOrEmpty(strDefaultSystem))
                {
                    sb.AppendFormat("{0}/{1}\n", profile.GetProfileId(), strDefaultSystem);
                }
            }

            return sb.ToString();
        }

        public void DeserializeState(string state)
        {
            if (string.IsNullOrEmpty(state))
                return;

            string[] profileStates = state.Split('\n');
            foreach (string profileState in profileStates)
            {
                if (string.IsNullOrEmpty(profileState) || (profileState.IndexOf('/') == -1))
                    continue;

                string[] values = profileState.Split('/');
                if ((values.Length == 0) ||
                    string.IsNullOrEmpty(values[0]) ||
                    string.IsNullOrEmpty(values[1]))
                    continue;

                foreach (CMtProfile profile in m_profileList)
                {
                    if (profile.IsProfile(values[0]))
                    {
                        profile.SetDefaultSystem(values[1]);
                        break;
                    }
                }
            }
        }
    }
}
