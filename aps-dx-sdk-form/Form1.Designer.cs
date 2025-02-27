
namespace aps_dx_sdk_form
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
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

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			button1 = new Button();
			textBox_clientid = new TextBox();
			label1 = new Label();
			textBox_clientsecret = new TextBox();
			label2 = new Label();
			label3 = new Label();
			textBox_hubid = new TextBox();
			label4 = new Label();
			label5 = new Label();
			textBox_filename = new TextBox();
			label6 = new Label();
			textBox_folderurn = new TextBox();
			label7 = new Label();
			textBox_projectid = new TextBox();
			SuspendLayout();
			// 
			// button1
			// 
			button1.Location = new Point(298, 549);
			button1.Name = "button1";
			button1.Size = new Size(188, 58);
			button1.TabIndex = 0;
			button1.Text = "Create Exchange";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// textBox_clientid
			// 
			textBox_clientid.Location = new Point(298, 51);
			textBox_clientid.Name = "textBox_clientid";
			textBox_clientid.Size = new Size(461, 47);
			textBox_clientid.TabIndex = 1;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(47, 48);
			label1.Name = "label1";
			label1.Size = new Size(131, 41);
			label1.TabIndex = 2;
			label1.Text = "Client ID";
			label1.Click += label1_Click;
			// 
			// textBox_clientsecret
			// 
			textBox_clientsecret.Location = new Point(298, 121);
			textBox_clientsecret.Name = "textBox_clientsecret";
			textBox_clientsecret.PasswordChar = '*';
			textBox_clientsecret.Size = new Size(461, 47);
			textBox_clientsecret.TabIndex = 3;
			// 
			// label2
			// 
			label2.AutoSize = true;
			label2.Location = new Point(47, 118);
			label2.Name = "label2";
			label2.Size = new Size(184, 41);
			label2.TabIndex = 4;
			label2.Text = "Client Secret";
			// 
			// label3
			// 
			label3.AutoSize = true;
			label3.Location = new Point(81, 198);
			label3.Name = "label3";
			label3.Size = new Size(0, 41);
			label3.TabIndex = 6;
			label3.Click += label3_Click;
			// 
			// textBox_hubid
			// 
			textBox_hubid.Location = new Point(298, 198);
			textBox_hubid.Name = "textBox_hubid";
			textBox_hubid.Size = new Size(461, 47);
			textBox_hubid.TabIndex = 5;
			textBox_hubid.TextChanged += textBox3_TextChanged;
			// 
			// label4
			// 
			label4.AutoSize = true;
			label4.Location = new Point(47, 198);
			label4.Name = "label4";
			label4.Size = new Size(107, 41);
			label4.TabIndex = 7;
			label4.Text = "Hub id";
			// 
			// label5
			// 
			label5.AutoSize = true;
			label5.Location = new Point(47, 278);
			label5.Name = "label5";
			label5.Size = new Size(150, 41);
			label5.TabIndex = 9;
			label5.Text = "File Name";
			// 
			// textBox_filename
			// 
			textBox_filename.Location = new Point(298, 278);
			textBox_filename.Name = "textBox_filename";
			textBox_filename.Size = new Size(461, 47);
			textBox_filename.TabIndex = 8;
			// 
			// label6
			// 
			label6.AutoSize = true;
			label6.Location = new Point(47, 356);
			label6.Name = "label6";
			label6.Size = new Size(234, 41);
			label6.TabIndex = 10;
			label6.Text = "ACC Folder URN";
			// 
			// textBox_folderurn
			// 
			textBox_folderurn.Location = new Point(298, 356);
			textBox_folderurn.Name = "textBox_folderurn";
			textBox_folderurn.Size = new Size(461, 47);
			textBox_folderurn.TabIndex = 11;
			// 
			// label7
			// 
			label7.AutoSize = true;
			label7.Location = new Point(47, 440);
			label7.Name = "label7";
			label7.Size = new Size(210, 41);
			label7.TabIndex = 12;
			label7.Text = "ACC Project ID";
			// 
			// textBox_projectid
			// 
			textBox_projectid.Location = new Point(298, 440);
			textBox_projectid.Name = "textBox_projectid";
			textBox_projectid.Size = new Size(461, 47);
			textBox_projectid.TabIndex = 13;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(17F, 41F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(823, 663);
			Controls.Add(textBox_projectid);
			Controls.Add(label7);
			Controls.Add(textBox_folderurn);
			Controls.Add(label6);
			Controls.Add(label5);
			Controls.Add(textBox_filename);
			Controls.Add(label4);
			Controls.Add(label3);
			Controls.Add(textBox_hubid);
			Controls.Add(label2);
			Controls.Add(textBox_clientsecret);
			Controls.Add(label1);
			Controls.Add(textBox_clientid);
			Controls.Add(button1);
			Name = "Form1";
			Text = "Data Exchange";
			Load += Form1_Load;
			ResumeLayout(false);
			PerformLayout();
		}

		private void label3_Click(object sender, EventArgs e)
		{
			throw new NotImplementedException();
		}

		#endregion

		private Button button1;
		private TextBox textBox_clientid;
		private Label label1;
		private TextBox textBox_clientsecret;
		private Label label2;
		private Label label3;
		private TextBox textBox_hubid;
		private Label label4;
		private Label label5;
		private TextBox textBox_filename;
		private Label label6;
		private TextBox textBox_folderurn;
		private Label label7;
		private TextBox textBox_projectid;
	}
}
