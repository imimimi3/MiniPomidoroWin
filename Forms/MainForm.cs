// SPDX-License-Identifier: GPL-3.0-or-later
// Copyright (C) 2026 imimimi3

using MiniPomidoroWin.Constants;
using MiniPomidoroWin.Forms;
using MiniPomidoroWin.Forms.Localization;
using MiniPomidoroWin.Service;
using MiniPomidoroWin.Themes;
using System.Diagnostics;
using System.Media;

namespace MiniPomidoroWin
{
	public partial class MainForm : LocalizedForm
	{
		/// <summary>
		/// РЎРµСЂРІРёСЃ Р·РІСѓРєР°
		/// </summary>
		private readonly AudioService _audioService = new AudioService();

		/// <summary>
		/// РћР±РЅРѕРІР»РµРЅРёРµ РїРѕРґСЃРєР°Р·РєРё Р·РЅР°С‡РєР° РІ С‚СЂРµРµ
		/// </summary>
		private readonly System.Windows.Forms.Timer _trayTooltipTimer =
			new System.Windows.Forms.Timer { Interval = AppConstants.TrayTooltipUpdateIntervalMs };

		/// <summary>
		/// РќР°РїРѕРјРёРЅР°РЅРёРµ, РєРѕРіРґР° С‚Р°Р№РјРµСЂ РЅРµ Р·Р°РїСѓС‰РµРЅ
		/// </summary>
		private readonly System.Windows.Forms.Timer _reminderTimer =
			new System.Windows.Forms.Timer { Interval = AppConstants.TrayReminderIntervalMs };

		/// <summary>
		/// РњРёРЅРё-РѕРєРЅРѕ СЃ С‚Р°Р№РјРµСЂРѕРј РѕР±СЂР°С‚РЅРѕРіРѕ РѕС‚СЃС‡С‘С‚Р°
		/// </summary>
		private MiniTimerForm? _miniTimerForm;

		/// <summary>
		/// РЎРѕРѕР±С‰РµРЅРёРµ РѕР± РѕРєРѕРЅС‡Р°РЅРёРё С‚Р°Р№РјРµСЂР°
		/// </summary>
		private NotificationForm? _notificationForm;

		/// <summary>
		/// РџСЂРёР»РѕР¶РµРЅРёРµ СЃРІС‘СЂРЅСѓС‚Рѕ РІ С‚СЂРµР№
		/// </summary>
		private bool _isInTray;

		/// <summary>
		/// РџСЂРёР»РѕР¶РµРЅРёРµ Р·Р°РІРµСЂС€Р°РµС‚ СЂР°Р±РѕС‚Сѓ
		/// </summary>
		private bool _exitRequested;

		/// <summary>
		/// РљРѕРЅСЃС‚СЂСѓРєС‚РѕСЂ С„РѕСЂРјС‹
		/// </summary>
		public MainForm()
		{
			InitializeComponent();

			this.BackColor = Theme.FormBackground;

			this.minutesField.ValueChanged += this.TimeField_ValueChanged;
			this.secondsField.ValueChanged += this.TimeField_ValueChanged;

			this._trayTooltipTimer.Tick += this.TrayTooltipTimer_Tick;
			this._reminderTimer.Tick += this.ReminderTimer_Tick;

			this._trayTooltipTimer.Start();
			this._reminderTimer.Start();

			this.ApplyLocalization();
			this.UpdateStartButtonState();
		}

		/// <summary>
		/// РџРµСЂРµС…РІР°С‚ Р·Р°РєСЂС‹С‚РёСЏ РіР»Р°РІРЅРѕР№ С„РѕСЂРјС‹: РєСЂРµСЃС‚РёРє СЃРІРѕСЂР°С‡РёРІР°РµС‚ РїСЂРёР»РѕР¶РµРЅРёРµ РІ С‚СЂРµР№
		/// </summary>
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			if (!this._exitRequested && e.CloseReason == CloseReason.UserClosing)
			{
				e.Cancel = true;
				this.HideMainFormToTray();
				return;
			}

			this._exitRequested = true;

			base.OnFormClosing(e);
		}

		/// <summary>
		/// РћСЃРІРѕР±РѕР¶РґРµРЅРёРµ СЂРµСЃСѓСЂСЃРѕРІ
		/// </summary>
		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			this._trayTooltipTimer.Stop();
			this._trayTooltipTimer.Dispose();

			this._reminderTimer.Stop();
			this._reminderTimer.Dispose();

			this._audioService.Dispose();

			if (this._notificationForm != null && !this._notificationForm.IsDisposed)
			{
				this._notificationForm.Close();
			}

			this.notifyIcon1.Visible = false;

			base.OnFormClosed(e);
		}

		/// <summary>
		/// РџСЂРёРјРµРЅРµРЅРёРµ Р»РѕРєР°Р»РёР·РѕРІР°РЅРЅС‹С… СЃС‚СЂРѕРє Рє СЌР»РµРјРµРЅС‚Р°Рј С„РѕСЂРјС‹
		/// </summary>
		protected override void ApplyLocalization()
		{
			this.Text = AppStrings.MainForm_Title;

			this.notifyMenuShow.Text = AppStrings.Tray_Show;
			this.notifyMenuExit.Text = AppStrings.Tray_Exit;

			this.minutesField.Caption = AppStrings.MainForm_MinutesShort;
			this.secondsField.Caption = AppStrings.MainForm_SecondsShort;

			this.UpdateTrayText();
		}

		/// <summary>
		/// РР·РјРµРЅРµРЅРёРµ РјРёРЅСѓС‚ РёР»Рё СЃРµРєСѓРЅРґ
		/// </summary>
		private void TimeField_ValueChanged(object? sender, EventArgs e)
		{
			this.UpdateStartButtonState();
		}

		/// <summary>
		/// РћР±С‰РµРµ РІСЂРµРјСЏ РІ СЃРµРєСѓРЅРґР°С…
		/// </summary>
		private int GetTotalSeconds()
		{
			return (this.minutesField.Value * 60) + this.secondsField.Value;
		}

		/// <summary>
		/// Р”РѕСЃС‚СѓРїРЅРѕСЃС‚СЊ РєРЅРѕРїРєРё РЎС‚Р°СЂС‚
		/// </summary>
		private void UpdateStartButtonState()
		{
			bool hasTime = this.GetTotalSeconds() > 0;

			this.btnStart.Enabled = hasTime;
			this.btnStart.BackColor = hasTime ? Theme.ButtonPrimary : Theme.FieldBackground;
			this.btnStart.ForeColor = hasTime ? Theme.TextColor : Theme.FieldCaption;
		}

		/// <summary>
		/// РћР±СЂР°Р±РѕС‚РєР° РЅР°Р¶Р°С‚РёСЏ РєРЅРѕРїРєРё РЎС‚Р°СЂС‚: Р·Р°РїСѓСЃРє С‚Р°Р№РјРµСЂР° РІ РјРёРЅРё-РѕРєРЅРµ
		/// </summary>
		private void btnStart_Click(object? sender, EventArgs e)
		{
			int seconds = this.GetTotalSeconds();

			if (seconds <= 0)
			{
				MessageBox.Show(
					this,
					AppStrings.MainForm_TimeZeroWarning,
					AppStrings.Common_WarningTitle,
					MessageBoxButtons.OK,
					MessageBoxIcon.Warning);

				return;
			}

			this._audioService.Stop();

			this.ShowMiniTimer(TimeSpan.FromSeconds(seconds));
		}

		/// <summary>
		/// РџРѕРєР°Р· РјРёРЅРё-РѕРєРЅР° Рё СЃРєСЂС‹С‚РёРµ РіР»Р°РІРЅРѕР№ С„РѕСЂРјС‹
		/// </summary>
		private void ShowMiniTimer(TimeSpan duration)
		{
			if (this._miniTimerForm != null && !this._miniTimerForm.IsDisposed)
			{
				this._isInTray = false;

				this.Hide();
				this._miniTimerForm.Show();
				this._miniTimerForm.Activate();
				this._miniTimerForm.BringToFront();

				this.UpdateTrayText();

				return;
			}

			Screen screen = Screen.FromControl(this);

			this._miniTimerForm = new MiniTimerForm(screen);
			this._miniTimerForm.TimerFinished += this.MiniTimerForm_TimerFinished;
			this._miniTimerForm.FormClosed += this.MiniTimerForm_FormClosed;
			this._miniTimerForm.CollapseRequested += this.MiniTimerForm_CollapseRequested;

			this._isInTray = false;

			this.Hide();

			this._miniTimerForm.Show();
			this._miniTimerForm.StartTimer(duration);

			this.UpdateTrayText();
		}

		/// <summary>
		/// РћРєРѕРЅС‡Р°РЅРёРµ С‚Р°Р№РјРµСЂР° РІ РјРёРЅРё-РѕРєРЅРµ
		/// </summary>
		private void MiniTimerForm_TimerFinished(object? sender, EventArgs e)
		{
			// РЎРЅР°С‡Р°Р»Р° СЃРѕРѕР±С‰РµРЅРёРµ, С‚РѕР»СЊРєРѕ РїРѕС‚РѕРј Р·РІСѓРє
			this.ShowTimerFinishedNotification();
			this.PlayEndSound();
		}

		/// <summary>
		/// РЎРІРѕСЂР°С‡РёРІР°РЅРёРµ РјРёРЅРё-РѕРєРЅР° РІ С‚СЂРµР№
		/// </summary>
		private void MiniTimerForm_CollapseRequested(object? sender, EventArgs e)
		{
			if (this._miniTimerForm == null || this._miniTimerForm.IsDisposed)
			{
				return;
			}

			this._isInTray = true;

			this._miniTimerForm.Hide();

			if (this.Visible)
			{
				this.Hide();
			}

			this.UpdateTrayText();
		}

		/// <summary>
		/// Р’РѕР·РІСЂР°С‚ РЅР° РіР»Р°РІРЅСѓСЋ С„РѕСЂРјСѓ РїРѕСЃР»Рµ Р·Р°РєСЂС‹С‚РёСЏ РјРёРЅРё-РѕРєРЅР°
		/// </summary>
		private void MiniTimerForm_FormClosed(object? sender, FormClosedEventArgs e)
		{
			if (this._miniTimerForm != null)
			{
				this._miniTimerForm.TimerFinished -= this.MiniTimerForm_TimerFinished;
				this._miniTimerForm.FormClosed -= this.MiniTimerForm_FormClosed;
				this._miniTimerForm.CollapseRequested -= this.MiniTimerForm_CollapseRequested;
				this._miniTimerForm = null;
			}

			if (this._exitRequested || this.IsDisposed || this.Disposing)
			{
				return;
			}

			this.UpdateTrayText();

			if (this._isInTray)
			{
				return;
			}

			this.Show();

			if (this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState = FormWindowState.Normal;
			}

			this.Activate();
			this.BringToFront();
			this.UpdateStartButtonState();
		}

		/// <summary>
		/// РћР±РЅРѕРІР»РµРЅРёРµ РїРѕРґСЃРєР°Р·РєРё Р·РЅР°С‡РєР° РІ С‚СЂРµРµ
		/// </summary>
		private void TrayTooltipTimer_Tick(object? sender, EventArgs e)
		{
			this.UpdateTrayText();
		}

		/// <summary>
		/// РўРµРєСЃС‚ РїРѕРґСЃРєР°Р·РєРё: РѕСЃС‚Р°РІС€РµРµСЃСЏ РІСЂРµРјСЏ РёР»Рё В«Р“РѕС‚РѕРІ Рє С‚СЂСѓРґСѓВ»
		/// </summary>
		private void UpdateTrayText()
		{
			string text = AppStrings.Tray_Ready;

			if (this._miniTimerForm is { IsDisposed: false } miniTimer && miniTimer.IsTimerActive)
			{
				text = string.Format(AppStrings.Tray_TimeLeftFormat, FormatTime(miniTimer.TimeLeft));
			}

			if (!string.Equals(this.notifyIcon1.Text, text, StringComparison.Ordinal))
			{
				this.notifyIcon1.Text = text;
			}
		}

		/// <summary>
		/// Р¤РѕСЂРјР°С‚РёСЂРѕРІР°РЅРёРµ РѕСЃС‚Р°РІС€РµРіРѕСЃСЏ РІСЂРµРјРµРЅРё
		/// </summary>
		private static string FormatTime(TimeSpan value)
		{
			return value.TotalHours >= 1
				? value.ToString(@"hh\:mm\:ss")
				: value.ToString(@"mm\:ss");
		}

		/// <summary>
		/// РќР°РїРѕРјРёРЅР°РЅРёРµ СЂР°Р· РІ 10 РјРёРЅСѓС‚, РµСЃР»Рё С‚Р°Р№РјРµСЂ РЅРµ Р·Р°РїСѓС‰РµРЅ
		/// </summary>
		private void ReminderTimer_Tick(object? sender, EventArgs e)
		{
			if (this._miniTimerForm is { IsDisposed: false } miniTimer && miniTimer.IsTimerActive)
			{
				return;
			}

			this.ShowTrayNotification(AppStrings.Tray_Reminder);
		}

		/// <summary>
		/// Р’СЃРїР»С‹РІР°СЋС‰РµРµ СѓРІРµРґРѕРјР»РµРЅРёРµ РёР· С‚СЂРµСЏ
		/// </summary>
		private void ShowTrayNotification(string message)
		{
			this.notifyIcon1.BalloonTipTitle = AppStrings.MainForm_Title;
			this.notifyIcon1.BalloonTipText = message;
			this.notifyIcon1.BalloonTipIcon = ToolTipIcon.Info;
			this.notifyIcon1.ShowBalloonTip(5000);
		}

		/// <summary>
		/// Р”РІРѕР№РЅРѕР№ РєР»РёРє РїРѕ Р·РЅР°С‡РєСѓ РІ С‚СЂРµРµ
		/// </summary>
		private void notifyIcon1_MouseDoubleClick(object? sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.RestoreFromTray();
			}
		}

		/// <summary>
		/// РџСѓРЅРєС‚ РјРµРЅСЋ В«Р Р°Р·РІРµСЂРЅСѓС‚СЊВ»
		/// </summary>
		private void notifyMenuShow_Click(object? sender, EventArgs e)
		{
			this.RestoreFromTray();
		}

		/// <summary>
		/// РџСѓРЅРєС‚ РјРµРЅСЋ В«Р’С‹С…РѕРґВ»
		/// </summary>
		private void notifyMenuExit_Click(object? sender, EventArgs e)
		{
			this._exitRequested = true;
			this.notifyIcon1.Visible = false;

			Application.Exit();
		}

		/// <summary>
		/// Р Р°Р·РІРѕСЂР°С‡РёРІР°РЅРёРµ РїСЂРёР»РѕР¶РµРЅРёСЏ РёР· С‚СЂРµСЏ
		/// </summary>
		private void RestoreFromTray()
		{
			if (this._exitRequested)
			{
				return;
			}

			this._isInTray = false;

			if (this._miniTimerForm is { IsDisposed: false } miniTimer)
			{
				if (!miniTimer.Visible)
				{
					miniTimer.Show();
				}

				if (miniTimer.WindowState == FormWindowState.Minimized)
				{
					miniTimer.WindowState = FormWindowState.Normal;
				}

				miniTimer.Activate();
				miniTimer.BringToFront();

				this.UpdateTrayText();

				return;
			}

			this.Show();

			if (this.WindowState == FormWindowState.Minimized)
			{
				this.WindowState = FormWindowState.Normal;
			}

			this.Activate();
			this.BringToFront();

			this.UpdateStartButtonState();
			this.UpdateTrayText();
		}

		/// <summary>
		/// РЎРєСЂС‹С‚РёРµ РіР»Р°РІРЅРѕР№ С„РѕСЂРјС‹ РІ С‚СЂРµР№
		/// </summary>
		private void HideMainFormToTray()
		{
			this._isInTray = true;
			this.Hide();
			this.UpdateTrayText();
		}

		/// <summary>
		/// Р’РѕСЃРїСЂРѕРёР·РІРµРґРµРЅРёРµ РјРµР»РѕРґРёРё РѕРєРѕРЅС‡Р°РЅРёСЏ С‚Р°Р№РјРµСЂР°
		/// </summary>
		private void PlayEndSound()
		{
			// Р¤Р°Р№Р» РјРѕРі РёСЃС‡РµР·РЅСѓС‚СЊ СЃ РґРёСЃРєР° вЂ” С‚РѕРіРґР° Р·РІСѓС‡РёС‚ РІСЃС‚СЂРѕРµРЅРЅР°СЏ РјРµР»РѕРґРёСЏ
			if (SettingsService.HasCustomSound && SettingsService.CustomSoundPath is string path)
			{
				try
				{
					this._audioService.PlayCustomFile(path);
					return;
				}
				catch (Exception ex)
				{
					MessageBox.Show(
						this,
						string.Format(AppStrings.MainForm_AudioPlaybackError, ex.Message),
						AppStrings.Common_ErrorTitle,
						MessageBoxButtons.OK,
						MessageBoxIcon.Error);
				}
			}

			try
			{
				this._audioService.PlayEmbedded(Properties.Resources.DefaultEndTimerMusic);
			}
			catch (Exception ex)
			{
				// РњРµР»РѕРґРёСЏ РЅРµ Р·Р°РёРіСЂР°Р»Р° вЂ” С…РѕС‚СЏ Р±С‹ СЃРёСЃС‚РµРјРЅС‹Р№ Р·РІСѓРє Рё Р·Р°РїРёСЃСЊ РІ Р»РѕРі
				Trace.TraceError("РќРµ СѓРґР°Р»РѕСЃСЊ РІРѕСЃРїСЂРѕРёР·РІРµСЃС‚Рё РІСЃС‚СЂРѕРµРЅРЅСѓСЋ РјРµР»РѕРґРёСЋ: {0}", ex);
				SystemSounds.Exclamation.Play();
			}
		}

		/// <summary>
		/// РЎРѕРѕР±С‰РµРЅРёРµ В«РџРѕСЂР° РїРµСЂРµРєР»СЋС‡РёС‚СЊСЃСЏ!В» РїРѕРІРµСЂС… РІСЃРµС… РѕРєРѕРЅ
		/// </summary>
		private void ShowTimerFinishedNotification()
		{
			if (this._exitRequested || this.IsDisposed || this.Disposing)
			{
				return;
			}

			if (this._notificationForm != null && !this._notificationForm.IsDisposed)
			{
				this._notificationForm.Close();
			}

			NotificationForm notification = new NotificationForm();
			notification.FormClosed += this.NotificationForm_FormClosed;

			this._notificationForm = notification;

			notification.Show();
			notification.Refresh();
			notification.Activate();
			notification.BringToFront();
		}

		/// <summary>
		/// Р—Р°РєСЂС‹С‚РёРµ СЃРѕРѕР±С‰РµРЅРёСЏ
		/// </summary>
		private void NotificationForm_FormClosed(object? sender, FormClosedEventArgs e)
		{
			if (sender is NotificationForm notification)
			{
				notification.FormClosed -= this.NotificationForm_FormClosed;
			}

			if (ReferenceEquals(this._notificationForm, sender))
			{
				this._notificationForm = null;
			}
		}

		/// <summary>
		/// РћР±СЂР°Р±РѕС‚С‡РёРє РЅР°Р¶Р°С‚РёСЏ РєРЅРѕРїРєРё РќР°СЃС‚СЂРѕР№РєРё
		/// </summary>
		private void btnSettings_Click(object? sender, EventArgs e)
		{
			using (SettingsForm settings = new SettingsForm())
			{
				settings.ShowDialog(this);
			}
		}

		/// <summary>
		/// РћР±СЂР°Р±РѕС‚С‡РёРє РЅР°Р¶Р°С‚РёСЏ РєРЅРѕРїРєРё РРЅС„РѕСЂРјР°С†РёСЏ
		/// </summary>
		private void btnInfo_Click(object? sender, EventArgs e)
		{
			using (InfoForm info = new InfoForm())
			{
				info.ShowDialog(this);
			}
		}
	}
}