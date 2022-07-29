using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PandaLyrics
{
    partial class SettingForm : Form
    {
        private const DialogResult oK = System.Windows.Forms.DialogResult.OK;
        AppSetting appSetting;
        MainWindow main;
        internal SettingForm(MainWindow main)
        {
            this.appSetting = main.appSetting;
            this.main = main;
            InitializeComponent();
        }

        private void SettingForm_Load(object sender, EventArgs e)
        {
            selectShadowColor.BackColor = (Color)new ColorConverter().ConvertFromString(appSetting.ShadowColor.ToString());
            selectFontColor.BackColor = (Color)new ColorConverter().ConvertFromString(appSetting.FontColor.ToString());
            selectFont.Text = appSetting.FontFamily.Source + "," + appSetting.FontSize.ToString() + "pt";
            selectFont.Font = new Font(appSetting.FontFamily.Source, 11, FontStyle.Bold);
            selectBgVisible.Checked = appSetting.BgVisible;
            selectStartUp.Checked = Utils.GetStartup();
            selectWidth.Value = (int)(appSetting.BgWidth / 1000.0 * 100.0);
            selectBgColor.BackColor = (Color)new ColorConverter().ConvertFromString(appSetting.BgColor.ToString());
            selectBgAlpha.Value = (int)(appSetting.BgAlpha * 100);
            selectWinOpacity.Value = (int)(appSetting.WinOpacity * 100);
        }

        private void shadowColorSelcect_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                string hexColor = ColorTranslator.ToHtml(cd.Color);
                appSetting.ShadowColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColor);
                appSetting.Save();

                selectShadowColor.BackColor = cd.Color;
            }
        }

        private void selectFont_Click(object sender, EventArgs e)
        {
            FontDialog fd = new FontDialog();

            fd.Font = new Font(
               appSetting.FontFamily.Source,
               (float)appSetting.FontSize,
               FontStyle.Bold
            );

            if (fd.ShowDialog() == oK)
            {
                appSetting.FontSize = fd.Font.Size;
                appSetting.FontFamily = new System.Windows.Media.FontFamily(fd.Font.Name);
                appSetting.Save();

                selectFont.Text = appSetting.FontFamily.Source + "," + appSetting.FontSize.ToString() + "pt";
                selectFont.Font = new Font(appSetting.FontFamily.Source, 11, FontStyle.Bold);
            }
        }

        private void selectFontColorClick(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                string hexColor = ColorTranslator.ToHtml(cd.Color);
                appSetting.FontColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColor);
                appSetting.Save();

                selectFontColor.BackColor = cd.Color;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void slelctBgVisible_CheckedChanged(object sender, EventArgs e)
        {
            appSetting.BgVisible = selectBgVisible.Checked;
            appSetting.Save();
        }

        private void selectStartUp_CheckedChanged(object sender, EventArgs e)
        {
            Utils.SetStartup(selectStartUp.Checked);
        }

        private void selectWidth_Scroll(object sender, EventArgs e)
        {
            appSetting.BgWidth = 1000.0 * (selectWidth.Value / 100.0);
            main.Width = appSetting.BgWidth;
            appSetting.Save();
        }

        private void selectBgColor_Click(object sender, EventArgs e)
        {
            ColorDialog cd = new ColorDialog();

            if (cd.ShowDialog() == DialogResult.OK)
            {
                string hexColor = ColorTranslator.ToHtml(cd.Color);
                appSetting.BgColor = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(hexColor);
                appSetting.Save();

                selectBgColor.BackColor = cd.Color;
            }
        }

        private void selectBgAlpha_Scroll(object sender, EventArgs e)
        {
            appSetting.BgAlpha = (selectBgAlpha.Value / 100.0);
            appSetting.Save();
        }

        private void selectWinOpacity_Scroll(object sender, EventArgs e)
        {
            appSetting.WinOpacity = (selectWinOpacity.Value / 100.0);
            main.Opacity = appSetting.WinOpacity;
            appSetting.Save();
        }
    }
}
