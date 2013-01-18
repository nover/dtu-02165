using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace bowling.scheduling
{
    public class LaneSchedulerState
    {
        int[,] state;
        public int numberOfLanes;
        public int numberOfTimeSlots;
        double[] weight;

        public int[,] State
        {
            get
            {
                return this.state;
            }
            set
            {
                this.state = value;
            }
        }

        public LaneSchedulerState(int numberOfLanes, int numberOfTimeSlots, List<LaneSchedulerReservation> reservations)
        {
            this.numberOfLanes = numberOfLanes;
            this.numberOfTimeSlots = numberOfTimeSlots;
            this.state = new int[numberOfTimeSlots, numberOfLanes];
            this.weight = new double[numberOfTimeSlots];

            foreach (LaneSchedulerReservation reservation in reservations)
            {
                for (int i = reservation.StartTimeSlot; i < reservation.StartTimeSlot + reservation.NumberOfTimeSlots; i++)
                {
                    for (int j = 0; j < reservation.NumberOfLanes; j++)
                    {
                        if (i < numberOfTimeSlots)
                        {
                            this.weight[i] += 1.0f;
                        }
                    }
                }
            }
        }

        public void Apply(LaneSchedulerAction action)
        {
            for (int i = action.lowestTimeSlot; i < action.lowestTimeSlot + action.numTimeSlots; i++)
            {
                for (int j = action.leftmostLane; j < action.leftmostLane + action.numLanes; j++)
                {
                    if (i < this.numberOfTimeSlots && j < this.numberOfLanes)
                    {
                        this.state[i, j] = action.reservation.Id;
                    }
                }
            }
        }

        public void Unapply(LaneSchedulerAction action)
        {
            for (int i = action.lowestTimeSlot; i < action.lowestTimeSlot + action.numTimeSlots; i++)
            {
                for (int j = action.leftmostLane; j < action.leftmostLane + action.numLanes; j++)
                {
                    if (i < this.numberOfTimeSlots && j < this.numberOfLanes)
                    {
                        this.state[i, j] = 0;
                    }
                }
            }
        }

        public bool IsPossible(LaneSchedulerReservation reservation)
        {
            for (int i = reservation.StartTimeSlot; i < reservation.NumberOfTimeSlots + reservation.StartTimeSlot; i++)
            {
                if (i >= this.numberOfTimeSlots)
                {
                    return false;
                }
                int numFreeCells = 0;
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    if (this.state[i, j] == 0)
                    {
                        numFreeCells++;
                    }
                }
                if (numFreeCells < reservation.NumberOfLanes)
                {
                    return false;
                }
            }
            return true;
        }

        public AppWeightPair IsApplicable(int lane, int numLanes, int numTimeSlots, int startTimeSlot)
        {
            //Debug.WriteLine("Checking for a reservation for " + numLanes + " lanes at lane: " + lane + " at timeslot: " + startTimeSlot);
            double weight = 0.0f;
            for (int i = startTimeSlot; i < startTimeSlot + numTimeSlots; i++)
            {
                if (i >= this.numberOfTimeSlots)
                {
                    return new AppWeightPair(false, 0);
                }
                for (int j = lane; j < lane + numLanes; j++)
                {
                    if (j >= this.numberOfLanes || this.state[i, j] != 0)
                    {
                        return new AppWeightPair(false, 0);
                    }
                    else
                    {
                        //weight = weight + this.getCellWeight(i);
                        //Debug.WriteLine("Weight updated with: " + this.getCellWeight(i) + " to: " + weight);
                    }
                }
                weight = Math.Max(weight, this.GetCellWeight(i));
            }
            weight += 1 / numLanes;
            //Debug.WriteLine("    Weight starts at: " + weight);
            // Various weight-changing rules for ensuring spread and guests sharing computers and ball-returns
            // Even numbered numLanes should be placed an even-numbered lane.
            if (numLanes % 2 == 0 && lane % 2 == 0)
            {
                weight += 1.0f;
            }
            // Take wear and tear into account. Use the reciprocal of the combined wear values for all used lanes, to ensure the combined lowest get the highest value.
            int wear = LaneWearData.GetWearForLanes(lane, numLanes);
            if (wear == 0)
            {
                wear = 1;
            }
            double wearValue = 1.0f / wear;
            weight += wearValue;

            // Try to spread reservations out
            // Get combined distance to other reservations
            double distance = this.GetCombinedDistanceToOthers(lane, numLanes, startTimeSlot);
            // Favour those with most distance to others - but not too much.
            //Debug.WriteLine("    Weight before distance: " + weight + " distance was: " + distance);

            weight += 1 / (distance);

            if ((startTimeSlot - 1 >= 0) && this.state[startTimeSlot - 1, lane] != 0)
            {
                weight = weight * 0.5f;
            }
            //Debug.WriteLine("    Weight ends at: " + weight);
            return new AppWeightPair(true, weight);
        }

        public double GetCombinedDistanceToOthers(int lane, int numLanes, int startTimeSlot)
        {
            double distanceRight = 0.0f;
            double distanceLeft = 0.0f;
            // get right distance
            for (int i = lane + 1; i < this.numberOfLanes; i++)
            {
                //Debug.WriteLine("    Checking pos right: " + startTimeSlot + "," + i + " with value: " + this.state[startTimeSlot, i]);
                if (i == this.numberOfLanes - 1)
                {
                    distanceRight = this.numberOfLanes;
                    break;
                }
                if (this.state[startTimeSlot, i] != 0)
                {
                    distanceRight = i - lane - 1;
                    //Debug.WriteLine("        and distance-right: " + i + " - " + lane + " = " + distanceRight);
                    break;
                }
            }
            // get left distance
            for (int i = lane - 1; i >= 0; i--)
            {
                //Debug.WriteLine("    Checking pos left: " + startTimeSlot + "," + i + " with value: " + this.state[startTimeSlot, i]);
                if (i <= 0)
                {
                    break;
                }
                if (this.state[startTimeSlot, i] != 0)
                {
                    distanceLeft = lane - i - 1;
                    //Debug.WriteLine("        and distance-left: " + lane + " - " + i + " = " + distanceLeft);
                    break;
                }
            }

            double distance = Math.Min(distanceRight, distanceLeft);
            //Debug.WriteLine("        Returning distance " + distanceRight + " <min> " + distanceLeft + "  -> " + distance);
            return distance;
        }

        public double GetCellWeight(int timeslot)
        {
            return this.weight[timeslot];
        }

        public string ToString()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = this.numberOfTimeSlots - 1; i >= 0; i--)
            {
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    string numRepr;
                    if (this.state[i, j] == 0)
                    {
                        numRepr = "...";
                    }
                    else if (this.state[i, j] < 10)
                    {
                        numRepr = "00" + this.state[i, j];
                        //numRepr = "" + this.getCellWeight(i);
                    }
                    else if (this.state[i, j] < 100)
                    {
                        numRepr = "0" + this.state[i, j];
                        //numRepr = "" + this.getCellWeight(i);
                    }
                    else
                    {
                        numRepr = "" + this.state[i, j];
                        //numRepr = "" + this.getCellWeight(i);
                    }

                    builder.Append("  " + numRepr);
                }
                builder.Append("\n");
            }
            return builder.ToString();
        }

        public double GetReservationWeight(LaneSchedulerReservation reservation)
        {
            double weight = 0.0f;

            for (int i = reservation.StartTimeSlot; i < reservation.StartTimeSlot + reservation.NumberOfTimeSlots; i++)
            {
                weight = Math.Max(weight, this.GetCellWeight(i));
            }
            weight += 1 / reservation.NumberOfLanes;
            return weight;
        }

        public String Repr()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < this.numberOfTimeSlots; i++)
            {
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    builder.Append(this.state[i, j]);
                }
            }
            return builder.ToString();
        }

        public String ReservationRepr(LaneSchedulerReservation reservation)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(reservation.Id);
            builder.Append(",");
            builder.Append(reservation.NumberOfLanes);
            builder.Append(",");
            builder.Append(reservation.NumberOfTimeSlots);
            builder.Append(",");
            builder.Append(reservation.StartTimeSlot);
            builder.Append("->");
            for (int i = reservation.StartTimeSlot - reservation.NumberOfTimeSlots; i < reservation.StartTimeSlot + reservation.NumberOfTimeSlots; i++)
            {
                if (i < this.numberOfTimeSlots && i >= 0)
                {
                    for (int j = 0; j < this.numberOfLanes; j++)
                    {
                        builder.Append(this.state[i, j]);
                        builder.Append(".");
                    }
                    builder.Append("/");
                }
            }
            return builder.ToString();
        }

        public List<LaneSchedulerState> CutInPieces(LaneSchedulerReservation reservation)
        {
            List<LaneSchedulerState> stateList = new List<LaneSchedulerState>();

            // detect cutting lines
            // find cutting line for upper part
            int upperCut = -1;
            for (int i = reservation.StartTimeSlot + reservation.NumberOfTimeSlots; i < this.numberOfTimeSlots; i++)
            {
                bool foundAny = false;
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    if (this.state[i, j] == this.state[i + 1, j] || this.state[i, j] == this.state[i - 1, j])
                    {
                        foundAny = true;
                    }
                    if (!foundAny)
                    {
                        upperCut = i;
                        break;
                    }
                }
            }
            // find cutting line for lower part
            int lowerCut = -1;
            for (int i = reservation.StartTimeSlot - 1; i >= 0; i--)
            {
                bool foundAny = false;
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    if (this.state[i, j] == this.state[i + 1, j] || this.state[i, j] == this.state[i - 1, j])
                    {
                        foundAny = true;
                    }
                    if (!foundAny)
                    {
                        lowerCut = i;
                        break;
                    }
                }
            }

            // do the cutting
            LaneSchedulerState stateUpper = new LaneSchedulerState(this.numberOfLanes, this.numberOfTimeSlots, new List<LaneSchedulerReservation>());
            stateUpper.weight = this.weight;
            stateUpper.weight = this.weight;
            stateUpper.state = new int[numberOfTimeSlots, numberOfLanes];

            LaneSchedulerState stateMiddle = new LaneSchedulerState(this.numberOfLanes, this.numberOfTimeSlots, new List<LaneSchedulerReservation>());
            stateMiddle.weight = this.weight;
            stateMiddle.weight = this.weight;
            stateMiddle.state = new int[numberOfTimeSlots, numberOfLanes];

            LaneSchedulerState stateLower = new LaneSchedulerState(this.numberOfLanes, this.numberOfTimeSlots, new List<LaneSchedulerReservation>());
            stateLower.weight = this.weight;
            stateLower.weight = this.weight;
            stateLower.state = new int[numberOfTimeSlots, numberOfLanes];

            // Populate them

            for (int i = 0; i < this.numberOfTimeSlots; i++)
            {
                for (int j = 0; j < this.numberOfLanes; j++)
                {
                    if (i <= lowerCut)
                    {
                        stateLower.state[i, j] = this.state[i, j];
                    }
                    else if (j >= upperCut)
                    {
                        stateMiddle.state[i, j] = this.state[i, j];
                    }
                    else
                    {
                        stateUpper.state[i, j] = this.state[i, j];
                    }
                }
            }
            stateList.Add(stateUpper);
            stateList.Add(stateMiddle);
            stateList.Add(stateLower);
            return stateList;
        }
    }
}
