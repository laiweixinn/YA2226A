using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using System.Text;

namespace LZ.CNC.UserLevel
{
    public enum LoginTypes
    {
        None,
        Operator,
        Engineer,
        Manufacturer
    }

    [Serializable]
    public class UserManagement
    {
        #region "密码参数"
        private LoginTypes _LoginType;

        private  string _PasswordUser = "1";

        private  string _PasswordEngineer = "2";

        private  string _PasswordManufacturer = "3";

        public  string PasswordUser
        {
            get
            {
                return _PasswordUser;
            }
            set
            {
                _PasswordUser = value;
            }
        }

        public  string PasswordEngineer
        {
            get
            {
                return _PasswordEngineer;
            }
            set
            {
                _PasswordEngineer = value;
            }
        }

        public  string PasswordManufacturer
        {
            get
            {
                return _PasswordManufacturer;
            }
            set
            {
                _PasswordManufacturer = value;
            }
        }

        public LoginTypes LoginType
        {
            get
            {
                return _LoginType;
            }
            set
            {
                _LoginType = value;
            }
        }

        public string LoginTypeToString
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                switch (_LoginType)
                {
                    case LoginTypes.None:
                        sb.Append("----");
                        break;
                    case LoginTypes.Operator:
                        sb.Append("普通用户");
                        break;
                    case LoginTypes.Engineer:
                        sb.Append("工程师");
                        break;
                    case LoginTypes.Manufacturer:
                        sb.Append("厂家");
                        break;
                    default:
                        break;
                }
                return sb.ToString();
            }
        }
        #endregion

        [field: NonSerialized]
        public event EventHandler LoginTypeChanged;

        private void OnLoginTypeChanged()
        {
            if (LoginTypeChanged!=null)
            {
                LoginTypeChanged(this, null);
            }
        }

        public bool LoginIn(bool login, UserManagement uesr)
        {
            if ((new LoginForm(login,uesr)).ShowDialog() != DialogResult.OK)
            {
                return false;
            }
            OnLoginTypeChanged();
            return true;
        }

        public void LoginOut()
        {
            if (_LoginType!=LoginTypes.None)
            {
                _LoginType = LoginTypes.None;
                OnLoginTypeChanged();
            }
        }

        public void ChangePassword()
        {

        }

        public UserManagement InitPassword()
        {
            UserManagement uesr = null;
            string path = Path.Combine(Application.StartupPath, "set");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path=Path.Combine(Application.StartupPath, "set/password.config");

            if (File.Exists(path))
            {
                FileStream fileStream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                uesr = (binaryFormatter.Deserialize(fileStream) as UserManagement);
                fileStream.Close();
            }

            if (uesr == null)
            {
                uesr = new UserManagement();
            }
            Save();
            return uesr;
        }

        internal void Save()
        {
            MemoryStream memoryStream = new MemoryStream();
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(memoryStream, this);
            string filepath= Path.Combine(Application.StartupPath, "set/password.config");
            FileStream fileStream = File.Open(filepath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
            memoryStream.WriteTo(fileStream);
            fileStream.Close();
            memoryStream.Close();
        }
    }
}
