using System;
using System.Collections.Generic;
using System.Text;
using Sdl.LanguagePlatform.Core;

namespace LetsMT.MTProvider
{
    public class CMtProfile
    {
        //Default system for this profile, could be empty
        private string m_defaultSystem;
        //All systems in this profile
        private List<CMtSystem> m_availableSystems;
        /// <summary>
        /// Default term corpora for each system. Shold not be modified modified using <see cref="M:SetDefaultTermCorpora(string, string)"/>.
        /// </summary>
        public Dictionary<string, string> m_defaultTermCorpora { get; set; }


        public string SourceLanguageId { get; set; }

        public string SourceLanguageName { get; set; }

        public string TargetLanguageId { get; set; }

        public string TargetLanguageName { get; set; }

        public static string GenerateProfileId(string sourceLanguageId, string targetLanguageId)
        {
            return string.Format("{0} - {1}", sourceLanguageId, targetLanguageId);
        }

        //Contains profile id like "en - lv"
        public string m_profileId
        {
            get { return GenerateProfileId(SourceLanguageId, TargetLanguageId); }
        }

        //Friendly name for listbox etc.
        public string m_profileFriendlyName
        {
            get { return string.Format("{0} - {1}", SourceLanguageName, TargetLanguageName); }
        }
        

        public CMtProfile(string sourceLanguageId, string sourceLanguageName, string targetLanguageId, string targetLanguageName)
        {
            this.SourceLanguageId = sourceLanguageId;
            this.SourceLanguageName = sourceLanguageName;
            this.TargetLanguageId = targetLanguageId;
            this.TargetLanguageName = targetLanguageName;

            m_defaultSystem = "";
            //Empty list
            m_availableSystems = new List<CMtSystem>();

            m_defaultTermCorpora = new Dictionary<string,string>();
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
                if (system.GetOnlineStatus() == "Running" || system.GetOnlineStatus() == "Queuing" || system.GetOnlineStatus() == "Standby")
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

        /// <summary>
        /// Gets id of the default term corpora for a given system.
        /// </summary>
        /// <param name="systemId">Id of the system for which to retrieve the default corpora.</param>
        /// <returns>Id of the default term corpora or "".</returns>
        public string GetDefaultTermCorpora(string systemId)
        {
            return m_defaultTermCorpora.GetValueOrDefault(systemId, "");
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

        public void SetDefaultTermCorpora(string systemId, string corporaId)
        {
            foreach (CMtSystem system in m_availableSystems)
            {
                if (system.IsSystem(systemId))
                {
                    m_defaultTermCorpora[systemId] = corporaId; // TODO: check if corporaId is valid
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
