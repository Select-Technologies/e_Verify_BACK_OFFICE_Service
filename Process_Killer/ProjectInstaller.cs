using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;

namespace e_Verify_BACK_OFFICE_Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        private void Service_Killer1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}