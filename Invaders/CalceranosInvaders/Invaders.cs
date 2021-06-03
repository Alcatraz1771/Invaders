using System;
using System.Windows.Forms;


namespace CalceranosInvaders
{
	public class Invaders
	{
		private readonly Panel invadersPanel;
		private readonly Panel invadersPanel1;
		private int addX;
		private int addY;
		private int minX;
		private int maxX;
		private int minY;
		private int maxY;
		private int delay;

		private int delayCounter;

		private int bottomRow;

		private int invadersXDistance;
		private int invadersYDistance;
		private bool isActive;
		private readonly int[] invadersToKillForColumn = new int[11];
		private readonly int[] invadersToKillForRow = new int[5];
		private readonly System.Collections.Generic.List<int> columnsForShooting;
		private readonly bool[,] isInvaderAlive = new bool[5, 11];

		private bool changeDirectionAndGoDown;
		private readonly Random random;
		private int invadersWidth;
		private int invadersHeight;

		public Invaders(Panel invadersPanel, Panel invadersPanel1)
		{
			this.invadersPanel = invadersPanel;
			this.invadersPanel1 = invadersPanel1;
			this.invadersPanel1.SendToBack();
			this.invadersPanel.SendToBack();
			columnsForShooting = new System.Collections.Generic.List<int>();
			random = new Random();
			Stop();
		}

		public void Start(int x, int y, int addX, int addY, int minX, int maxX, int minY, int maxY, int delay)
		{
			X0 = x;
			Y0 = y;
			this.addX = addX;
			this.addY = addY;
			this.minX = minX;
			this.maxX = maxX;
			this.minY = minY;
			this.maxY = maxY;
			this.delay = delay;
			delayCounter = 0;
			invadersPanel.Left = X0;
			invadersPanel.Top = Y0;
			LeftColumn = 0;
			RightColumn = 10;
			InvadersToKill = 55;
			IsComplete = false;
			invadersXDistance = invadersPanel.Controls["lb_Invader01"].Left -
								invadersPanel.Controls["lb_Invader00"].Left;
			invadersYDistance = invadersPanel.Controls["lb_Invader10"].Top - invadersPanel.Controls["lb_Invader00"].Top;
			invadersPanel1.Visible = false;
			invadersPanel.Visible = false;
			Reset();
			invadersPanel1.SendToBack();
			invadersPanel.Visible = true;
			invadersPanel1.Visible = false;
			isActive = true;
		}


		public void Freeze()
		{
			isActive = false;
		}

		public void Defreeze()
		{
			isActive = true;
		}

		public void Stop()
		{
			isActive = false;
			invadersPanel.Visible = false;
			invadersPanel1.Visible = false;
		}

		public void Move()
		{
			if (!isActive)
			{
				return;
			}


			if (delayCounter < delay)
			{
				delayCounter += 1;
				return;
			}

			delayCounter = 0;
			if (changeDirectionAndGoDown)
			{
				addX = -addX;
				Y0 += addY;
				changeDirectionAndGoDown = false;
			}
			else
			{
				X0 += addX;
				if (X0 < minX)
				{
					X0 = minX;
					changeDirectionAndGoDown = true;
				}
				else if (X0 > maxX)
				{
					X0 = maxX;
					changeDirectionAndGoDown = true;
				}
			}

			invadersPanel.Left = X0;
			invadersPanel.Top = Y0;
			invadersPanel1.Left = X0;
			invadersPanel1.Top = Y0;
			if (invadersPanel1.Visible)
			{
				invadersPanel1.SendToBack();
				invadersPanel.Visible = true;
				invadersPanel1.Visible = false;
			}
			else
			{
				invadersPanel.SendToBack();
				invadersPanel1.Visible = true;
				invadersPanel.Visible = false;
			}
		}

		public int BottomX0ForColumn(int column)
		{
			for (var i = 4; i >= 0; i -= 1)
			{
				if (!isInvaderAlive[i, column]) continue;
				var columnString = column == 10 ? "A" : column.ToString();

				return X0 + invadersPanel.Controls["lb_Invader" + i + columnString].Left;
			}

			return -1;
		}

		public int BottomX1ForColumn(int column)
		{
			for (var i = 4; i >= 0; i -= 1)
			{
				if (!isInvaderAlive[i, column]) continue;
				var columnString = column == 10 ? "A" : column.ToString();

				return X0 + invadersPanel.Controls["lb_Invader" + i + columnString].Left +
						invadersPanel.Controls["lb_Invader" + i + columnString].Width;
			}

			return -1;
		}

		public int BottomY0ForColumn(int column)
		{
			for (var i = 4; i >= 0; i -= 1)
			{
				if (!isInvaderAlive[i, column]) continue;
				var columnString = column == 10 ? "A" : column.ToString();

				return Y0 + invadersPanel.Controls["lb_Invader" + i + columnString].Top;
			}

			return -1;
		}

		public int BottomY1ForColumn(int column)
		{
			for (var i = 4; i >= 0; i -= 1)
			{
				if (!isInvaderAlive[i, column]) continue;
				var columnString = column == 10 ? "A" : column.ToString();

				return Y0 + invadersPanel.Controls["lb_Invader" + i + columnString].Top +
						invadersPanel.Controls["lb_Invader" + i + columnString].Height;
			}

			return -1;
		}


		public void StartInvadersBullet(Bullet bullet)
		{
			if (!isActive)
			{
				return;
			}

			var columnIndexer = random.Next(0, columnsForShooting.Count - 1);
			var column = columnsForShooting[columnIndexer];
			for (var i = 4; i >= 0; i -= 1)
			{
				if (!isInvaderAlive[i, column]) continue;
				
				var columnString = column == 10 ? "A" : column.ToString();

				bullet.Start(X0 + invadersPanel.Controls["lb_Invader" + i + columnString].Left,
							Y0 + invadersPanel.Controls["lb_Invader" + i + columnString].Top +
							invadersPanel.Controls["lb_Invader" + i + columnString].Height,
							invadersPanel.Parent.Height, 0, 6);
				break;
			}
		}

		public int X0 { get; private set; }

		public int X1 => X0 + invadersWidth;

		public int Y0 { get; private set; }

		public int Y1 => Y0 + invadersHeight;

		private int LeftColumn { get; set; }

		private int RightColumn { get; set; }

		public bool IsComplete { get; private set; }

		public int Delay
		{
			get => delay;
			set => delay = value < 0 ? 0 : value;
		}

		public int InvadersToKill { get; private set; }

		private void CheckBottomRow()
		{
			var rowsCompleted = 0;
			do
			{
				var isRowCompleted = (invadersToKillForRow[bottomRow] == 0);
				if (isRowCompleted)
				{
					rowsCompleted += 1;
					bottomRow -= 1;
				}
				else break;
			} while (true);

			if (rowsCompleted > 0) invadersHeight -= invadersYDistance * rowsCompleted;
		}

		private void CheckLeftOrRightColumn(bool checkingForLeft)
		{
			int column;
			int addColumn;
			var columnsCompleted = 0;

			if (checkingForLeft)
			{
				column = LeftColumn;
				addColumn = 1;
			}
			else
			{
				column = RightColumn;
				addColumn = -1;
			}

			do
			{
				var isColumnCompleted = (invadersToKillForColumn[column] == 0);
				if (isColumnCompleted)
				{
					columnsCompleted += 1;
					column += addColumn;
				}
				else
				{
					break;
				}
			} while (true);

			if (columnsCompleted <= 0) return;
			if (checkingForLeft)
			{
				LeftColumn = column;
				minX -= invadersXDistance * columnsCompleted;
			}
			else
			{
				RightColumn = column;
				maxX += invadersXDistance * columnsCompleted;
				invadersWidth -= invadersXDistance * columnsCompleted;
			}
		}


		private void Reset()
		{
			for (var i = 0; i <= 4; i = i + 1)
			{
				var row = i.ToString();
				for (var j = LeftColumn; j <= RightColumn; j += 1)
				{
					var column = j == 10 ? "A" : j.ToString();

					invadersPanel.Controls["lb_Invader" + row + column].Visible = true;
					invadersPanel1.Controls["label" + ((5 - i) + (10 - j) * 5)].Visible = true;
					isInvaderAlive[i, j] = true;
				}
			}

			for (var i = 0; i <= 4; i += 1)
			{
				invadersToKillForRow[i] = 11;
			}

			for (var j = LeftColumn; j <= RightColumn; j += 1)
			{
				invadersToKillForColumn[j] = 5;
				columnsForShooting.Add(j);
			}

			invadersWidth = invadersPanel.Width;
			bottomRow = 4;
			invadersHeight = invadersPanel.Height;
		}

		public int CheckHit(int x, int y)
		{
			var i = (y - Y0) / 22;
			var j = (x - X0) / 34;
			if (i < 0 || i > 4 || j < LeftColumn || j > RightColumn)
			{
				return 0;
			}

			var row = i.ToString();
			var column = j == 10 ? "A" : j.ToString();

			if (!isInvaderAlive[i, j]) return 0;
			if (x < invadersPanel.Controls["lb_Invader" + row + column].Left + X0 || x >
				invadersPanel.Controls["lb_Invader" + row + column].Left + X0 +
				invadersPanel.Controls["lb_Invader" + row + column].Width ||
				y < invadersPanel.Controls["lb_Invader" + row + column].Top + Y0 || y >
				invadersPanel.Controls["lb_Invader" + row + column].Top + Y0 +
				invadersPanel.Controls["lb_Invader" + row + column].Height) return 0;
			invadersPanel.Controls["lb_Invader" + row + column].Visible = false;
			invadersPanel1.Controls["label" + ((5 - i) + (10 - j) * 5)].Visible = false;
			isInvaderAlive[i, j] = false;
			invadersToKillForColumn[j] -= 1;
			invadersToKillForRow[i] -= 1;

			InvadersToKill -= 1;
			var points = 0;
			switch (i)
			{
				case 4:
				case 3:
					points = 10;
					break;
				case 2:
				case 1:
					points = 20;
					break;
				case 0:
					points = 40;
					break;
			}

			if (InvadersToKill == 0)
			{
				IsComplete = true;
				return points;
			}

			if (invadersToKillForColumn[j] == 0) columnsForShooting.Remove(j);

			if (j == LeftColumn) CheckLeftOrRightColumn(true);
			else if (j == RightColumn) CheckLeftOrRightColumn(false);

			if (i == bottomRow) CheckBottomRow();
			return points;

		}
	}
}