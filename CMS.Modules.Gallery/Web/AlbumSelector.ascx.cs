using System;
using System.ComponentModel;
using System.Web.UI;

namespace CMS.Modules.Gallery.Web
{
    public partial class AlbumSelectorControl : System.Web.UI.UserControl
    {
        protected string _clear;
        protected string _processModalData;
        protected string _showModal;

        private string _unassigned;

        [DefaultValue("")]
        public string UnassignedText
        {
            get
            {
                if (_unassigned != null)
                {
                    return _unassigned;
                }
                return "";
            }
            set { _unassigned = value; }
        }

        public int SelectedAlbumId
        {
            get
            {
                if (string.IsNullOrEmpty(hiddenId.Value))
                {
                    return 0;
                }
                return Convert.ToInt32(hiddenId.Value);
            }
            set
            {
                hiddenId.Value = value.ToString();
            }
        }

        /// <summary>
        /// Please set after selected user id
        /// </summary>
        public string SelectedAlbum
        {
            get { return textBoxAlbumName.Text; }
            set { labelAlbumName.Text = value; }
        }

        public int SectionId;
        public int NodeId;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                _clear = this.ClientID + "_clear";
                _processModalData = this.ClientID + "_process";
                _showModal = this.ClientID + "_showModal";

                #region -- Popup center --

                const string centerPopup =
                    @"function PopupCenter(pageURL, title,h,w) {
var left = (screen.width/2)-(w/2);
var top = (screen.height/2)-(h/2);
var targetWin = window.open (pageURL, title, 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width='+w+', height='+h+', top='+top+', left='+left);
targetWin.focus();
return targetWin;
}";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "centerPopup", centerPopup, true);
                //Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "centerPopup", centerPopup, true);

                #endregion

                #region -- Get Query String --

                const string getQuery =
                    @"function getQueryString( name )
{
  name = name.replace(/[\[]/,""\\\["").replace(/[\]]/,""\\\]"");
  var regexS = ""[\\?&]""+name+""=([^&#]*)"";
  var regex = new RegExp( regexS );
  var results = regex.exec( window.location.href );
  if( results == null )
    return """";
  else
    return results[1];
}";
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "getQuery", getQuery, true);

                #endregion

                #region -- Show Modal Popup --

                string showModal = string.Format(@"function {0}()
{{
PopupCenter('/Modules/Gallery/AlbumSelectorPage.aspx?command={1}&SectionId={2}&NodeId={3}',640,480);
}}", _showModal, _processModalData, SectionId, NodeId);
                ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), this.ClientID + "_showModal", showModal,
                                                        true);

                #endregion

                #region -- Clear --

                string clear =
                    string.Format(@"function {0}()
{{
self.document.getElementById('{1}').innerHTML  = '{2}';
self.document.getElementById('{3}').value  = '';
self.document.getElementById('{4}').disabled = true;
self.document.getElementById('{5}').style.display = 'inline';
}}", _clear, labelAlbumName.ClientID, UnassignedText, hiddenId.ClientID, btnRemove.ClientID, textBoxAlbumName.ClientID);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), this.ClientID + "_clear", clear, true);

                #endregion

                #region -- ProcessModalData --

                string processModal =
                    string.Format(@"
function {0}( userId, username )
{{
self.focus();
self.document.getElementById('{1}').innerHTML  = username;
self.document.getElementById('{2}').value  = userId;
self.document.getElementById('{3}').disabled = false;
self.document.getElementById('{4}').style.display = 'none';
}}", _processModalData, labelAlbumName.ClientID, hiddenId.ClientID, btnRemove.ClientID, textBoxAlbumName.ClientID);
                Page.ClientScript.RegisterClientScriptBlock(this.GetType(), this.ClientID + "_process", processModal,
                                                            true);

                #endregion

                btnSelect.Attributes.Add("onClick", _showModal + "()");
                btnRemove.Attributes.Add("onClick", _clear + "()");

                if (SelectedAlbumId > 0)
                {
                    hiddenId.Value = SelectedAlbumId.ToString();
                    textBoxAlbumName.Style["display"] = "none";
                }
                else
                {
                    btnRemove.Attributes.Add("disabled", "true");
                }
            }
        }
    }
}