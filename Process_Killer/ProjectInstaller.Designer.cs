namespace e_Verify_BACK_OFFICE_Service
{
    partial class ProjectInstaller
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.e_Verify_BACK_OFFICE_Service = new System.ServiceProcess.ServiceProcessInstaller();
            this.e_Verify_BACK_OFFICE_Service_Windows_Service = new System.ServiceProcess.ServiceInstaller();
            // 
            // Interface_UP_File_Uploads_Service
            // 
            this.e_Verify_BACK_OFFICE_Service.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.e_Verify_BACK_OFFICE_Service.Password = null;
            this.e_Verify_BACK_OFFICE_Service.Username = null;
            // 
            // Interface_UP_File_Uploads_Windows_Service
            // 
            this.e_Verify_BACK_OFFICE_Service_Windows_Service.DisplayName = "e_Verify_BACK_OFFICE_Service";
            this.e_Verify_BACK_OFFICE_Service_Windows_Service.ServiceName = "e_Verify_BACK_OFFICE_Service";
            this.e_Verify_BACK_OFFICE_Service_Windows_Service.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.e_Verify_BACK_OFFICE_Service.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.Service_Killer1_AfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.e_Verify_BACK_OFFICE_Service,
            this.e_Verify_BACK_OFFICE_Service_Windows_Service});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller e_Verify_BACK_OFFICE_Service;
        private System.ServiceProcess.ServiceInstaller e_Verify_BACK_OFFICE_Service_Windows_Service;
    }
}