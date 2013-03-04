using System;
using System.Collections.Generic;
using System.Text;
using Sdl.LanguagePlatform.Core;

namespace LetsMT.MTProvider
{
    public class CMtProfile
    {
        //Contains profile id like "en - lv"
        private string m_profileId;
        //Default system for this profile, could be empty
        private string m_defaultSystem;
        //Friendly name for listbox etc.
        private string m_profileFriendlyName;
        //All systems in this profile
        private List<CMtSystem> m_availableSystems;

        public CMtProfile(string strProfileId, string strProfileFriendlyName)
        {
            m_profileId = strProfileId;
            m_profileFriendlyName = strProfileFriendlyName;
            m_defaultSystem = "";
            //Empty list
            m_availableSystems = new List<CMtSystem>();
        }

        //Returns true if this is the matching profile
        public bool IsProfile(string profileId)
        {
            return (m_profileId == profileId) ? true : false;
        }

        //Used to interop with Trados Language pair
        public bool IsProfile(LanguagePair profileId)
        {
            string profile = string.Format("{0} - {1}", profileId.SourceCulture.TwoLetterISOLanguageName, profileId.TargetCulture.TwoLetterISOLanguageName);

            return IsProfile(profile);
        }

        //Checks if profile has running systems
        public bool HasOnlineSystems()
        {
            bool bHasRunningSystem = false;

            foreach (CMtSystem system in m_availableSystems)
            {
                if (system.GetOnlineStatus() == "Running")
                {
                    bHasRunningSystem = true;
                    break;
                }
            }

            return bHasRunningSystem;
        }

        //Gets list item for listbox
        public ListItem GetListItem()
        {
            return new ListItem(m_profileFriendlyName, m_profileId);
        }

        //Gets id of the default system or ""
        public string GetDefaultSystem()
        {
            return m_defaultSystem;
        }

        //Sets default system if system exists
        public void SetDefaultSystem(string strSystem)
        {
            if (strSystem == "")
            {
                m_defaultSystem = "";
                return;
            }

            foreach (CMtSystem system in m_availableSystems)
            {
                if (system.IsSystem(strSystem))
                {
                    m_defaultSystem = strSystem;
                    break;
                }
            }
        }

        //Returns profile id for serialization etc.
        public string GetProfileId()
        {
            return m_profileId;
        }

        //Adds a system to available system list for this profile, returns reference to the system
        public CMtSystem AddSystem(string strSystemId, string strFriendlyName, string strFriendlyDescription, string strOnlineStatus)
        {
            CMtSystem system = new CMtSystem(strSystemId, strFriendlyName, strFriendlyDescription, strOnlineStatus);

            m_availableSystems.Add(system);

            //if (m_defaultSystem == "")
                //m_defaultSystem = strSystemId;

            return system;
        }

        //Gets all systems available for this profile as list items
        public List<ListItem> GetSystemList(bool bFiltered)
        {
            List<ListItem> systems = new List<ListItem>();

            foreach(CMtSystem system in m_availableSystems)
            {
                string systemStatus = system.GetOnlineStatus();
                if (!bFiltered || systemStatus == "Running" || systemStatus == "Standby" || systemStatus == "Queuing")
                    systems.Add(system.GetListItem());
            }

            return systems;
        }
    }
}
