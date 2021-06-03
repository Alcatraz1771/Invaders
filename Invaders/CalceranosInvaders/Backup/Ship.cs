using System;
using System.Windows.Forms;

namespace CalceranosInvaders
{
	public class Ship
	{
		private int minX;
		private int maxX;
		private int addX;
		private readonly Label shipLabel;


		public Ship(Label shipLabel)
		{
			this.shipLabel = shipLabel;
			Stop();
		}

		public void Start(int x, int y, int minX, int maxX, int addX)
		{
			X = x;
			Y = y;
			this.minX = minX;
			this.maxX = maxX;
			this.addX = addX;
			shipLabel.Left = X;
			shipLabel.Top = Y;
			shipLabel.Visible = true;
			IsActive = true;
		}

		public void Freeze()
		{
			IsActive = false;
		}

		public void Defreeze()
		{
			IsActive = true;
		}

		public void Stop()
		{
			shipLabel.Visible = true;
			IsActive = false;
		}

		public void MoveLeft()
		{
			if (!IsActive) return;

			X -= addX;
			if (X < minX) X = minX;

			shipLabel.Left = X;
		}

		public void MoveRight()
		{
			if (!IsActive) return;

			X += addX;
			if (X > maxX) X = maxX;

			shipLabel.Left = X;
		}

		public int X { get; private set; }

		public int Y { get; private set; }

		public bool IsActive { get; private set; }
	}
}