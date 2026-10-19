
using System;
using System.Drawing;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace SmtpClientApp
{
    public partial class Form1 : Form
    {
        TextBox txtFrom = new TextBox();
        TextBox txtPassword = new TextBox();
        TextBox txtTo = new TextBox();
        TextBox txtSubject = new TextBox();
        TextBox txtMessage = new TextBox();
        Button btnSend = new Button();

        public Form1()
        {
            InitializeComponent();

            Text = "SMTP Email Client";
            Size = new Size(500, 550);

            AddLabel("Ваш email:", 20, 20);
            txtFrom.SetBounds(150, 20, 300, 25);

            AddLabel("Пароль приложения:", 20, 60);
            txtPassword.SetBounds(150, 60, 300, 25);
            txtPassword.UseSystemPasswordChar = true;

            AddLabel("Кому:", 20, 100);
            txtTo.SetBounds(150, 100, 300, 25);

            AddLabel("Тема:", 20, 140);
            txtSubject.SetBounds(150, 140, 300, 25);

            AddLabel("Сообщение:", 20, 180);
            txtMessage.SetBounds(150, 180, 300, 180);
            txtMessage.Multiline = true;
            txtMessage.ScrollBars = ScrollBars.Vertical;

            btnSend.Text = "Отправить письмо";
            btnSend.SetBounds(150, 390, 180, 40);
            btnSend.Click += BtnSend_Click;

            Controls.AddRange(new Control[]
            {
                txtFrom, txtPassword, txtTo,
                txtSubject, txtMessage, btnSend
            });
        }

        void AddLabel(string text, int x, int y)
        {
            Label label = new Label();
            label.Text = text;
            label.SetBounds(x, y, 125, 25);
            Controls.Add(label);
        }

        async void BtnSend_Click(object sender, EventArgs e)
        {
            try
            {
                btnSend.Enabled = false;

                using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                {
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(
                        txtFrom.Text,
                        txtPassword.Text
                    );

                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = new MailAddress(txtFrom.Text);
                        mail.To.Add(txtTo.Text);
                        mail.Subject = txtSubject.Text;
                        mail.Body = txtMessage.Text;

                        await smtp.SendMailAsync(mail);
                    }
                }

                MessageBox.Show("Письмо успешно отправлено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка отправки: " + ex.Message);
            }
            finally
            {
                btnSend.Enabled = true;
            }
        }
    }
}
