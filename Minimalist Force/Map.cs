using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace WindowsGame2
{
    internal class Map
    {
        public const int TILESIZEX = 60;
        public const int TILESIZEY = 48;
        // 2024.12.02 introduced constants
        public const int MAXX = 1 + Game1.PREFERREDBACKBUFFERWIDTH / TILESIZEX;
        public const int MAXY = 1 + Game1.PREFERREDBACKBUFFERHEIGHT / TILESIZEY;
        public const int MAP_MAXSIZEX = MAXX * 5 / 2;
        public const int MAP_MAXSIZEY = MAXY * 5 / 2;
        public const int MAXMAPSIZE = MAP_MAXSIZEX * MAP_MAXSIZEY;
        public const int MAPHEADERSIZE = 0;
        public const int MAXPATHLENGTH = 50;
        public const int OPACITY_FULL = 255;
        public const int OPACITY_LESS = 200;
        public const int MAXMAPFOOTERSIZE = 100;
        public const int ENDOFENEMYBLOCKTOKEN = 255;
        public const int BORDER = 2;
        public const int RADIUS1 = 3;
        public const int RADIUS2 = 4;
        public const int AVGNUMENEMIES = 10;
        public const int DEVNUMENEMIES = 4;
        public const int PROBABILITYDIFFICULTENEMY = 10;

        public enum Directions
        {
            Left,
            Right,
            Up,
            Down
        }

        public static Texture2D Texture;
        public Vector2 Size;
        public Vector2 Position;
        public Vector2 Offset = new Vector2(0, 0);
        public Directions[] currentPath = new Directions[MAXPATHLENGTH];
        public Directions[] bestPath = new Directions[MAXPATHLENGTH];

        private int bestPath_numberSteps;

        private bool[] mapViable = new bool[MAXMAPSIZE + MAPHEADERSIZE];
        private bool[] mapMarked = new bool[MAXMAPSIZE + MAPHEADERSIZE];

        private bool _blinkStatus;

        public bool BlinkStatus
        {
            get { return _blinkStatus; }
            set { _blinkStatus = value; }
        }

        private Random _rnd;

        public Random Rnd
        {
            get => _rnd;
        }

        public Map()
        {
            _rnd = new Random();
            Size = new Vector2(_rnd.Next(MAP_MAXSIZEX * 2 / 3, MAP_MAXSIZEX), _rnd.Next(MAP_MAXSIZEY * 2 / 3, MAP_MAXSIZEY));
            //Position = new Vector2(Size.X / 2, Size.Y / 2);
            EmptyMapViable();
            EmptyMapMarked();
        }

        public void EmptyMapViable()
        {
            for (int i = 0; i < MAXMAPSIZE + MAPHEADERSIZE; i++)
                mapViable[i] = false;
        }

        // 2023.04.03 Fixed bug
        public void CalcViable(float position, float move, bool flying, Party otherParty, Party neutralParty)
        {
            mapViable[(int)position] = true;

            if (move >= 1)
            {
                move--;

                if ((position - MAPHEADERSIZE) - Size.X > 0)
                    CalcViable_Helper(position - Size.X, move, flying, otherParty, neutralParty);
                if ((position - MAPHEADERSIZE) + Size.X < Size.Y * Size.X)
                    CalcViable_Helper(position + Size.X, move, flying, otherParty, neutralParty);
                if ((position - MAPHEADERSIZE) % Size.X != 0)
                    CalcViable_Helper(position - 1, move, flying, otherParty, neutralParty);
                if (position % Size.X != 0)
                    CalcViable_Helper(position + 1, move, flying, otherParty, neutralParty);
            }
        }

        private void CalcViable_Helper(float newPosition, float move, bool flying, Party otherParty, Party neutralParty)
        {
            int i;
            bool collision = false;

            if (newPosition >= MAPHEADERSIZE && newPosition < MAPHEADERSIZE + Size.X * Size.Y)
            {
                collision = (newPosition % Size.X == 0) || (newPosition + 1 % Size.X == 0)
                    || (newPosition / Size.X == 0) || (newPosition / Size.X >= Size.Y - 1);

                if (collision) return;

                for (i = 0; !collision && i < otherParty.Members.Count; i++)
                {
                    if (otherParty.Members[i].Position == newPosition && otherParty.Members[i].Alive == true)
                    {
                        collision = true;
                    }
                }

                if (collision) return;

                for (i = 0; !collision && i < neutralParty.Members.Count; i++)
                {
                    if (neutralParty.Members[i].Position == newPosition && neutralParty.Members[i].Alive == true)
                    {
                        collision = true;
                    }
                }

                if (collision) return;

                CalcViable(newPosition, move, flying, otherParty, neutralParty);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Draw(spriteBatch, Position, false, true);
        }

        public void Draw(SpriteBatch spriteBatch, bool displayMarked)
        {
            Draw(spriteBatch, Position, displayMarked, true);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 topLeft)
        {
            Draw(spriteBatch, topLeft, false, true);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 topLeft, bool displayMarked)
        {
            Draw(spriteBatch, topLeft, displayMarked, true);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 topLeft, bool displayMarked, bool displayViable)
        {
            Draw(spriteBatch, topLeft, displayMarked, displayViable, OPACITY_FULL);
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 topLeft, bool displayMarked, bool displayViable, byte opacity1)
        {
            float pointer;
            Vector2 tempOffset = Offset;
            Vector2 frame = new Vector2();
            if (Size.X < MAXX) frame.X = Size.X; else frame.X = MAXX;
            if (Size.Y < MAXY) frame.Y = Size.Y; else frame.Y = MAXY;
            if (tempOffset.X > 0)
            {
                tempOffset.X -= TILESIZEX;
                topLeft.X--;
            }
            if (tempOffset.Y > 0)
            {
                tempOffset.Y -= TILESIZEY;
                topLeft.Y--;
            }
            if (tempOffset.X < 0)
            {
                frame.X++;
                if (tempOffset.X < -TILESIZEX)
                    frame.X++;
            }
            if (tempOffset.Y < 0)
            {
                frame.Y++;
                if (tempOffset.Y < -TILESIZEY)
                    frame.Y++;
            }
            pointer = MAPHEADERSIZE + topLeft.Y * Size.X + topLeft.X;
            for (int i = 0; i < frame.Y; i++)
            {
                for (int j = 0; j < frame.X; j++)
                {
                    byte opacity = opacity1;
                    if (displayViable)
                    {
                        if (_blinkStatus && mapViable[(int)pointer])
                            opacity -= OPACITY_FULL - OPACITY_LESS;
                    }
                    if (displayMarked)
                    {
                        if (_blinkStatus && mapMarked[(int)pointer])
                            opacity -= OPACITY_FULL - OPACITY_LESS;
                    }
                    // 2024.12.02 fix to enable higher resolutions than the default 800x600
                    if (Texture != null)
                    {
                        spriteBatch.Draw(Texture, new Vector2(tempOffset.X + j * TILESIZEX, tempOffset.Y + i * TILESIZEY), new Color(opacity, opacity, opacity, opacity));
                    }
                    pointer++;
                }
                pointer += Size.X - frame.X;
            }
        }

        public Boolean InBoundaries(Vector2 newPos)
        {
            if (newPos.X < 0)
                return false;

            if (newPos.Y < 0)
                return false;

            if (newPos.X + MAXX >= Size.X)
                return false;

            if (newPos.Y + MAXY >= Size.Y)
                return false;

            return true;
        }

        public Boolean AtLeftBorder(Vector2 position)
        {
            if (position.X <= 0)
                return true;

            return false;
        }

        public Boolean AtRightBorder(Vector2 position)
        {
            if (position.X >= MAXX - 1)
                return true;

            return false;
        }

        public Boolean AtTopBorder(Vector2 position)
        {
            if (position.Y <= 0)
                return true;

            return false;
        }

        public Boolean AtBottomBorder(Vector2 position)
        {
            if (position.Y >= MAXY - 1)
                return true;

            return false;
        }

        public Vector2 CalcPosition(float position)
        {
            return CalcPosition(position, Position);
        }

        public Vector2 CalcPosition(float position, Vector2 topLeft)
        {
            Vector2 returnPos;
            float row = (int)((position - MAPHEADERSIZE) / Size.X);

            returnPos.Y = row - topLeft.Y;
            returnPos.X = position - MAPHEADERSIZE - row * Size.X - topLeft.X;

            return returnPos;
        }

        public bool IsViable(float position)
        {
            return mapViable[(int)position];
        }

        public bool IsVisible(float position, Vector2 topLeft)
        {
            Vector2 pos = CalcPosition(position, topLeft);

            if (pos.X < 0)
                return false;

            if (pos.Y < 0)
                return false;

            if (pos.X >= MAXX)
                return false;

            if (pos.Y >= MAXY)
                return false;

            return true;
        }

        public bool IsVisibleMinusBorder(float position, Vector2 topLeft)
        {
            Vector2 pos = CalcPosition(position, topLeft);

            if (pos.X < 3)
                return false;

            if (pos.Y < 3)
                return false;

            if (pos.X > MAXX - 3)
                return false;

            if (pos.Y > MAXY - 3)
                return false;

            return true;
        }

        public bool IsVisibleMinusTopBorder(float position, Vector2 topLeft)
        {
            Vector2 pos = CalcPosition(position, topLeft);

            if (pos.X < 0)
                return false;

            if (pos.Y < 3)
                return false;

            if (pos.X >= MAXX)
                return false;

            if (pos.Y >= MAXY)
                return false;

            return true;
        }

        public bool IsOccupied(float position, CharacterPointer characterPointer, Party party1, Party party2)
        {
            for (int i = 0; i < party1.Members.Count; i++)
                if (characterPointer.BelongsToSide != CharacterPointer.Sides.Player || i != characterPointer.WhichOne)
                    if (party1.Members[i].Alive && position == party1.Members[i].Position)
                        return true;

            for (int i = 0; i < party2.Members.Count; i++)
                if (characterPointer.BelongsToSide != CharacterPointer.Sides.CPU_Opponents || i != characterPointer.WhichOne)
                    if (party2.Members[i].Alive && position == party2.Members[i].Position)
                        return true;

            return false;
        }

        public bool AnyCharacterLocatedAt(float position, Party party)
        {
            for (int i = 0; i < party.Members.Count; i++)
                if (position == party.Members[i].Position && party.Members[i].HitPoints > 0)
                    return true;

            return false;
        }

        public int CharacterLocatedAt(float position, Party party)
        {
            for (int i = 0; i < party.Members.Count; i++)
                if (position == party.Members[i].Position && party.Members[i].HitPoints > 0)
                    return i;

            return -1; // none found
        }

        public void EmptyMapMarked()
        {
            for (int i = 0; i < MAXMAPSIZE + MAPHEADERSIZE; i++)
                mapMarked[i] = false;
        }

        public void MarkFieldsWithDistance(float position, int distance)
        {
            MarkFieldsWithDistance(position, distance, false, false, false, false);
        }

        public void MarkFieldsWithDistance(float position, int distance, bool leftConsumed, bool rightConsumed, bool upConsumed, bool downConsumed)
        {
            if (distance == 0)
            {
                mapMarked[(int)position] = true;
            }
            else if (distance == 1)
            {
                if (!rightConsumed && (position - MAPHEADERSIZE) % Size.X != 0 && position - 1 >= MAPHEADERSIZE)
                    mapMarked[(int)position - 1] = true;

                if (!leftConsumed && (position + 1 - MAPHEADERSIZE) % Size.X != 0 && position + 1 <= MAPHEADERSIZE + Size.X * Size.Y)
                    mapMarked[(int)position + 1] = true;

                if (!downConsumed && position - Size.X >= MAPHEADERSIZE)
                    mapMarked[(int)(position - Size.X)] = true;

                if (!upConsumed && position + Size.X < MAPHEADERSIZE + Size.X * Size.Y)
                    mapMarked[(int)(position + Size.X)] = true;
            }
            else
            {
                if (!rightConsumed && (position - MAPHEADERSIZE) % Size.X != 0 && position - 1 >= MAPHEADERSIZE)
                    MarkFieldsWithDistance(position - 1, distance - 1, true, rightConsumed, upConsumed, downConsumed);

                if (!leftConsumed && (position + 1 - MAPHEADERSIZE) % Size.X != 0 && position + 1 <= MAPHEADERSIZE + Size.X * Size.Y)
                    MarkFieldsWithDistance(position + 1, distance - 1, leftConsumed, true, upConsumed, downConsumed);

                if (!downConsumed && position - Size.X >= MAPHEADERSIZE)
                    MarkFieldsWithDistance(position - Size.X, distance - 1, leftConsumed, rightConsumed, true, downConsumed);

                if (!upConsumed && position + Size.X < MAPHEADERSIZE + Size.X * Size.Y)
                    MarkFieldsWithDistance(position + Size.X, distance - 1, leftConsumed, rightConsumed, upConsumed, true);
            }
        }

        public bool AnyCharacterLocatedInMarkedFields(Party party)
        {
            for (int i = 0; i < party.Members.Count; i++)
                if (party.Members[i].Alive && mapMarked[(int)party.Members[i].Position])
                    return true;

            return false;
        }

        public int GetNextCharacterPositionLocatedInMarkedFields(Party party, int skip)
        {
            for (int i = 0; i < party.Members.Count; i++)
                if (party.Members[i].Alive && mapMarked[(int)party.Members[i].Position])
                {
                    if (skip == 0)
                        return (int)party.Members[i].Position;
                    else
                        skip--;
                }

            return -1; // not found
        }

        public int GetNextCharacterNumberLocatedInMarkedFields(Party party, int skip)
        {
            for (int i = 0; i < party.Members.Count; i++)
                if (party.Members[i].Alive && mapMarked[(int)party.Members[i].Position])
                {
                    if (skip == 0)
                        return i;
                    else
                        skip--;
                }

            return -1; // not found
        }

        public int GetCharacterNumberLocatedInGivenPosition(Party party, int position)
        {
            for (int i = 0; i < party.Members.Count; i++)
                if (party.Members[i].Alive && (int)party.Members[i].Position == position)
                    return i;

            return -1; // not found
        }

        public void CalcBestPath(int currentPosition, int destinationPosition, int maxStepsToGo)
        {
            bestPath_numberSteps = -1;
            CalcBestPath_Helper(0, currentPosition, destinationPosition, maxStepsToGo);
        }

        public void CalcBestPath_Helper(int currentPositionNumber, int currentPosition, int destinationPosition, int maxStepsToGo)
        {
            int i;

            if (currentPosition == destinationPosition)
            {
                if (bestPath_numberSteps == -1 || bestPath_numberSteps > currentPositionNumber)
                {
                    bestPath_numberSteps = currentPositionNumber;
                    for (i = 0; i < currentPositionNumber; i++)
                    {
                        bestPath[i] = currentPath[i];
                    }
                }
            }
            else if (maxStepsToGo > 0)
            {
                if (currentPosition - Size.X >= MAPHEADERSIZE && mapViable[(int)(currentPosition - Size.X)])
                {
                    currentPath[currentPositionNumber] = Directions.Up;
                    CalcBestPath_Helper(currentPositionNumber + 1, (int)(currentPosition - Size.X), destinationPosition, maxStepsToGo - 1);
                }
                if ((currentPosition - MAPHEADERSIZE) % Size.X != 0 && currentPosition - 1 >= MAPHEADERSIZE && mapViable[currentPosition - 1])
                {
                    currentPath[currentPositionNumber] = Directions.Left;
                    CalcBestPath_Helper(currentPositionNumber + 1, currentPosition - 1, destinationPosition, maxStepsToGo - 1);
                }
                if ((currentPosition + 1 - MAPHEADERSIZE) % Size.X != 0 && currentPosition + 1 <= 1 + Size.X * Size.Y && mapViable[currentPosition + 1])
                {
                    currentPath[currentPositionNumber] = Directions.Right;
                    CalcBestPath_Helper(currentPositionNumber + 1, currentPosition + 1, destinationPosition, maxStepsToGo - 1);
                }
                if (currentPosition + Size.X < MAPHEADERSIZE + Size.X * Size.Y && mapViable[(int)(currentPosition + Size.X)])
                {
                    currentPath[currentPositionNumber] = Directions.Down;
                    CalcBestPath_Helper(currentPositionNumber + 1, (int)(currentPosition + Size.X), destinationPosition, maxStepsToGo - 1);
                }
            }
        }

        public int GetBestPathNumberSteps()
        {
            return bestPath_numberSteps;
        }

        public Vector2 CenterMapPosition(int characterPosition)
        {
            int x = (characterPosition - MAPHEADERSIZE) % (int)Size.X;
            int y = (characterPosition - MAPHEADERSIZE) / (int)Size.X;
            int mapPosition_x = x - MAXX / 2;
            int mapPosition_y = y - MAXY / 2;

            if (mapPosition_x < 0)
                mapPosition_x = 0;
            if (mapPosition_y < 0)
                mapPosition_y = 0;
            while (mapPosition_x + MAXX >= Size.X)  // *** >= oder >?
                mapPosition_x--;
            while (mapPosition_y + MAXY >= Size.Y)  // *** >= oder >?
                mapPosition_y--;

            return new Vector2(mapPosition_x, mapPosition_y);
        }

        public Vector2 CalcClosestMapPositionSoThatVisible(float targetPosition)
        {
            Vector2 targetVector;
            Vector2 returnPosition = Position;
            float row = (int)((targetPosition - MAPHEADERSIZE) / Size.X);

            targetVector.Y = row;
            targetVector.X = targetPosition - MAPHEADERSIZE - row * Size.X;

            if (returnPosition.X > targetVector.X)
                returnPosition.X = targetVector.X;
            else if (returnPosition.X < targetVector.X - MAXX + 1)
                returnPosition.X = targetVector.X - MAXX + 1;

            if (returnPosition.Y > targetVector.Y)
                returnPosition.Y = targetVector.Y;
            else if (returnPosition.Y < targetVector.Y - MAXY + 1)
                returnPosition.Y = targetVector.Y - MAXY + 1;

            return returnPosition;
        }

        public bool MarkAttackableOpponents(Party allies, Party opponents, int minAttackRange, int maxAttackRange)
        {
            bool opponentInRange = false;

            for (int i = MAPHEADERSIZE; i < MAPHEADERSIZE + (int)(Size.X * Size.Y); i++)
            {
                if (IsViable(i))
                {
                    bool collision = false;
                    for (int j = 0; !collision && j < allies.Members.Count; j++)
                        if (j != allies.MemberOnTurn && allies.Members[j].Position == i && allies.Members[j].Alive)
                            collision = true;

                    if (!collision)
                        for (int j = minAttackRange; j <= maxAttackRange; j++)
                        {
                            MarkFieldsWithDistance(i, j);
                            if (AnyCharacterLocatedInMarkedFields(opponents))
                                opponentInRange = true;
                        }
                }
            }

            return opponentInRange;
        }

        public bool IsMarked(float position)
        {
            return mapMarked[(int)position];
        }

        public int CalcPositionWhereToMove(Party enemies, Party party, int charNumber, int minAttackRange, int maxAttackRange)
        {
            int backUpPosition = -1;
            int bestPath_numberSteps = -1;
            int skip;

            for (int i = MAPHEADERSIZE; i < MAPHEADERSIZE + (int)(Size.X * Size.Y); i++)
            {
                if (IsViable(i))
                {
                    bool collision = false;
                    for (int j = 0; !collision && j < enemies.Members.Count; j++)
                        if (j != enemies.MemberOnTurn && enemies.Members[j].Position == i && enemies.Members[j].Alive)
                            collision = true;

                    if (!collision)
                    {
                        EmptyMapMarked();
                        for (int j = minAttackRange; j <= maxAttackRange; j++)
                        {
                            MarkFieldsWithDistance(i, j);
                            if (AnyCharacterLocatedInMarkedFields(party))
                            {
                                skip = 0;
                                while (charNumber != GetNextCharacterNumberLocatedInMarkedFields(party, skip) && GetNextCharacterPositionLocatedInMarkedFields(party, skip) != -1)
                                    skip++;
                                if (charNumber == GetNextCharacterNumberLocatedInMarkedFields(party, skip))
                                {
                                    CalcBestPath((int)enemies.Members[enemies.MemberOnTurn].Position, i, enemies.Members[enemies.MemberOnTurn].MovePoints);
                                    if (bestPath_numberSteps == -1
                                        || bestPath_numberSteps > GetBestPathNumberSteps())
                                    {
                                        backUpPosition = i;
                                        bestPath_numberSteps = GetBestPathNumberSteps();
                                    }
                                }
                            }
                        }
                    }
                }
            }

            // 2024.12.02 this should prevent occasional bugs where the enemy moves erratically and the game crashes
            return backUpPosition != -1 ? backUpPosition : (int)enemies.Members[enemies.MemberOnTurn].Position;
        }

        public void MarkViable()
        {
            for (int i = MAPHEADERSIZE; i < MAXMAPSIZE + MAPHEADERSIZE; i++)
                if (mapViable[i])
                    mapMarked[i] = true;
        }

        public void CalcViableFromMarked(int move, bool flying, Party otherParty, Party neutralParty)
        {
            for (int i = MAPHEADERSIZE; i < MAXMAPSIZE + MAPHEADERSIZE; i++)
                if (mapMarked[i])
                    CalcViable(i, move, flying, otherParty, neutralParty);
        }

        public int FollowBestPath(int currentPosition, Party currentParty)
        {
            return FollowBestPath(0, currentPosition, currentParty);
        }

        public int FollowBestPath(int currentPositionNumber, int currentPosition, Party currentParty)
        {
            int newPosition = currentPosition;

            switch (bestPath[currentPositionNumber])
            {
                case Directions.Up:
                    newPosition -= (int)Size.X;
                    break;

                case Directions.Left:
                    newPosition--;
                    break;

                case Directions.Right:
                    newPosition++;
                    break;

                case Directions.Down:
                    newPosition += (int)Size.X;
                    break;
            }

            if (IsViable(newPosition) && !IsOccupiedByParty(newPosition, currentParty))
                return FollowBestPath(currentPositionNumber + 1, newPosition, currentParty);
            else
                return currentPosition;
        }

        public bool IsOccupiedByParty(int newPosition, Party currentParty)
        {
            for (int i = 0; i < currentParty.Members.Count; i++)
                if (currentParty.Members[i].Position == newPosition)
                    return true;

            return false;
        }

        public bool MarkHealableAllies(Party allies, int minRange, int maxRange)
        {
            return MarkAttackableOpponents(allies, allies, minRange, maxRange);
        }

        public bool MarkRecruitableAllies(Party party, Party allies, int minRange, int maxRange)
        {
            return MarkAttackableOpponents(party, allies, minRange, maxRange);
        }

        public bool MustSurviveCharacterInProximity(Party allies, int minRange, int maxRange)
        {
            int skip;

            for (int i = MAPHEADERSIZE; i < MAPHEADERSIZE + (int)(Size.X * Size.Y); i++)
            {
                if (IsViable(i))
                {
                    bool collision = false;
                    for (int j = 0; !collision && j < allies.Members.Count; j++)
                        if (j != allies.MemberOnTurn && allies.Members[j].Position == i && allies.Members[j].Alive)
                            collision = true;

                    if (!collision)
                    {
                        EmptyMapMarked();
                        for (int j = minRange; j <= maxRange; j++)
                        {
                            MarkFieldsWithDistance(i, j);
                            if (AnyCharacterLocatedInMarkedFields(allies))
                            {
                                skip = 0;
                                while (GetNextCharacterPositionLocatedInMarkedFields(allies, skip) != -1
                                       && !allies.Members[GetNextCharacterNumberLocatedInMarkedFields(allies, skip)].MustSurvive)
                                    skip++;
                                if (GetNextCharacterPositionLocatedInMarkedFields(allies, skip) != -1
                                     && allies.Members[GetNextCharacterNumberLocatedInMarkedFields(allies, skip)].MustSurvive)
                                    return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public float GetStartingPositionOfPartyMember(Party party_player_or_enemies_or_neutral, int i)
        {
            var pos = new Vector2(0, 0);
            var pos_flat = 0;
            var needs_repeat = false;

            do
            {
                int x0, x1, y0, y1;

                if (i == 0)
                {
                    switch (party_player_or_enemies_or_neutral.Side)
                    {
                        case CharacterPointer.Sides.Player:
                            x0 = BORDER;
                            x1 = (int)Size.X / RADIUS1 - BORDER;
                            y0 = (int)Size.Y - (int)Size.Y / RADIUS1;
                            y1 = (int)Size.Y - 2 - BORDER;
                            break;

                        case CharacterPointer.Sides.CPU_Opponents:
                            x0 = (int)Size.X - (int)Size.X / RADIUS1;
                            x1 = (int)Size.X - 2 - BORDER;
                            y0 = BORDER;
                            y1 = (int)Size.Y / RADIUS1 - BORDER;
                            break;

                        case CharacterPointer.Sides.Neutral:
                        default:
                            x0 = BORDER;
                            x1 = (int)Size.X - 2 - BORDER;
                            y0 = BORDER;
                            y1 = (int)Size.Y - 2 - BORDER;
                            break;
                    }
                }
                else
                {
                    switch (party_player_or_enemies_or_neutral.Side)
                    {
                        case CharacterPointer.Sides.Player:
                        case CharacterPointer.Sides.CPU_Opponents:
                            int x = (int)party_player_or_enemies_or_neutral.Members[i - 1].Position % (int)Size.X;
                            int y = (int)party_player_or_enemies_or_neutral.Members[i - 1].Position / (int)Size.X;
                            x0 = x - (int)Size.X / RADIUS2;
                            x1 = x + (int)Size.X / RADIUS2;
                            y0 = y - (int)Size.Y / RADIUS2;
                            y1 = y + (int)Size.Y / RADIUS2;
                            if (x0 < BORDER) x0 = BORDER;
                            if (y0 < BORDER) y0 = BORDER;
                            if (x1 > Size.X - 2 - BORDER) x1 = (int)Size.X - 2 - BORDER;
                            if (y1 > Size.Y - 2 - BORDER) y1 = (int)Size.Y - 2 - BORDER;
                            if (x1 < BORDER) x1 = BORDER;
                            if (y1 < BORDER) y1 = BORDER;
                            if (x0 > Size.X - 2 - BORDER) x0 = (int)Size.X - 2 - BORDER;
                            if (y0 > Size.Y - 2 - BORDER) y0 = (int)Size.Y - 2 - BORDER;
                            break;

                        case CharacterPointer.Sides.Neutral:
                        default:
                            x0 = BORDER;
                            x1 = (int)Size.X - 2 - BORDER;
                            y0 = BORDER;
                            y1 = (int)Size.Y - 2 - BORDER;
                            break;
                    }
                }

                pos = new Vector2(_rnd.Next(Math.Min(x0, x1), Math.Max(x0, x1)), _rnd.Next(Math.Min(y0, y1), Math.Max(y0, y1)));
                pos_flat = (int)ConvertPositionFromVector(pos);

                needs_repeat = false;

                for (var j = 0; j < i; j++)
                {
                    if (party_player_or_enemies_or_neutral.Members[j].Position == pos_flat)
                    {
                        needs_repeat = true;
                        break;
                    }
                }
            }
            while (needs_repeat);

            return pos_flat;
        }

        public int GetNumberOfEnemies()
        {
            return _rnd.Next(AVGNUMENEMIES - DEVNUMENEMIES, AVGNUMENEMIES + DEVNUMENEMIES);
        }

        public int GetEnemyId(int i)
        {
            //1234567890
            if (i == 0) return (int)Character.Enemies.Samurai;
            //if (i == 0) return (int)Character.Enemies.Tower;

            var numPossibilities = Enum.GetNames(typeof(Character.Enemies)).Length;
            if (_rnd.Next(PROBABILITYDIFFICULTENEMY) >= PROBABILITYDIFFICULTENEMY - 1)
            {
                return _rnd.Next(numPossibilities);
            }
            else
            {
                return _rnd.Next(1 + numPossibilities / 2);
            }
        }

        public void AdjustPartyMemberPosition(Party party, int i)
        {
            party.Members[i].Position++;
            if (party.Members[i].Position / Size.X == 0)
            {
                party.Members[i].Position += Size.X;
            }
            if (party.Members[i].Position / Size.X >= Size.Y - 1)
            {
                party.Members[i].Position = 0;
            }
            if (party.Members[i].Position % Size.X < BORDER)
            {
                party.Members[i].Position += BORDER;
            }
            if (party.Members[i].Position % Size.X > Size.X - 2 - BORDER)
            {
                party.Members[i].Position -= BORDER;
            }
        }

        public float ConvertPositionFromVector(Vector2 position)
        {
            return MAPHEADERSIZE + position.Y * Size.X + position.X;
        }
    }
}
