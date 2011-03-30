using System;
using System.Collections.Generic;
using System.Text;

namespace LetsMT.MTProvider
{
    public class CMtSystem
    {
        //System id used for translation calls
        private string m_systemId;
        //User friendly name for listbox
        private string m_systemFriendlyName;
        //System description
        private string m_systemFriendlyDescription;
        //Online status
        private string m_systemOnlineStatus;

        public CMtSystem(string strSystemId, string strFriendlyName, string strFriendlyDescription, string strOnlineStatus)
        {
            m_systemId = strSystemId;
            m_systemFriendlyName = strFriendlyName;
            m_systemFriendlyDescription = strFriendlyDescription;
            m_systemOnlineStatus = strOnlineStatus;
        }

        //Returns true if this system has the needed id
        public bool IsSystem(string strSystemId)
        {
            return (m_systemId == strSystemId)?true:false;
        }

        //Returns friendly name
        public string GetName()
        {
            return m_systemFriendlyName;
        }

        //Return friendly description
        public string GetDescription()
        {
            return m_systemFriendlyDescription;
        }

        //Returns system as list item for listbox
        public ListItem GetListItem()
        {
            return new ListItem(string.Format("{0} ({1})", m_systemFriendlyName, m_systemOnlineStatus), m_systemId);
        }
    }
}
