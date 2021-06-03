using System;
using System.Windows.Forms;

namespace CalceranosInvaders
{
	public class Bullet
	{
		private Label bulletLabel;
		private int maxY;
		private int minY;
		private int addY;

		public Bullet(Label bulletLabel)
		{
			this.bulletLabel = bulletLabel;
			IsActive = false;
			this.bulletLabel.Visible = false;
		}

		public void Start(int x, int y, int maxY, int minY, int addY)
		{
			if (IsActive) return;

			X = x;
			Y = y;
			bulletLabel.Top = Y;
			bulletLabel.Left = X;
			this.maxY = maxY;
			this.minY = minY;
			this.addY = addY;
			IsActive = true;
			bulletLabel.Visible = true;
		}

		public void Stop()
		{
			IsActive = false;
			bulletLabel.Visible = false;
		}

		public void Move()
		{
			if (!IsActive) return;

			Y += addY;
			if (Y > maxY || Y < minY)
			{
				bulletLabel.Visible = false;
				IsActive = false;
				return;
			}

			bulletLabel.Top = Y;
		}

		public bool IsActive { get; private set; }

		public int X { get; private set; }

		public int Y { get; private set; }
	}
}