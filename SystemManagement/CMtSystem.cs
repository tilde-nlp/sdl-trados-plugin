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

        public CMtSystem(string strSystemId, string strFriendlyName, string strFriendlyDescription)
        {
            m_systemId = strSystemId;
            m_systemFriendlyName = strFriendlyName;
            m_systemFriendlyDescription = strFriendlyDescription;
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
            return new ListItem(m_systemFriendlyName, m_systemId);
        }
    }
}
