using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Diagnostics.Contracts;

/// <summary>
/// Theatre application for all performances using code contracts interface
/// </summary>
namespace Theatre
{
    /// <summary>
    /// Interface class gives the basic set up of the implementation class outlining the functions and properties used
    /// </summary>
    [ContractClass(typeof(CTheatre))]
    interface ITheatre
    {
        /// <summary>
        /// Each performance in a theatre will hold the available tickets value
        /// </summary>
        int available { get; }
        /// <summary>
        /// An array of the Seat struct allows each seat to hold the state and customer
        /// </summary>
        Seat[] seats { get; }

        int max { get; }

        bool reserveSeat(int customer, int seat);
        bool buySeat(int customer, int seat);
        bool returnSeat(int customer, int seat);
        bool cancelReservations();
        int checkAvailability();
        Seat checkCustomer(int seat);
        Seat[] printSeatingPlan(); 
        void removeReserved();


    }

    /// <summary>
    /// Abstract class holds the contracts the implementation class must satisfy
    /// </summary>
    [ContractClassFor(typeof(ITheatre))]
    abstract class CTheatre : ITheatre
    {
        public int available
        {
            get
            {
                return default(int);
            }
        }

        public int max
        {
            get
            {
                return default(int);
            }
        }

        public Seat[] seats
        {
            get
            {
                return default(Seat[]);
            }
        }



        public bool buySeat(int customer, int seat)
        {
            Contract.Requires(customer > 0);
            Contract.Requires(seat > 0 && seat <= max);
            //if seat is bought then the seat state will be purchased and contain the customer, else the seat should not contain the customer
            Contract.Ensures(true ? seats[seat].state.Equals(state.purchased) : seats[seat].customer != customer);
            return default(bool);
        }

        public bool cancelReservations()
        {
            Contract.Ensures(available.Equals(max));
            return default(bool);
        }

        public int checkAvailability()
        {
            return default(int);
        }

        public Seat checkCustomer(int seat)
        {
            Contract.Requires(seat > 0 && seat <= max);
            return default(Seat);
        }

        public Seat[] printSeatingPlan()
        {
            return default(Seat[]);
        }

        public void removeReserved()
        {
            
        }

        public bool reserveSeat(int customer, int seat)
        {
            //customer number must be above 0 and the seat number must be in the theatre
            Contract.Requires(customer > 0);
            Contract.Requires(seat > 0 && seat < max);
            //the availability must always be more then 6 before the performance
            Contract.Ensures(available >= (max*0.04));
            Contract.Ensures(true ? seats[seat].state == state.reserved : seats[seat].customer != customer);
            return default(bool);
        }

        public bool returnSeat(int customer, int seat)
        {
            Contract.Requires(available < max);
            Contract.Requires(customer > 0);
            Contract.Requires(seat > 0 && seat < max);
            Contract.Ensures(true ? seats[seat].customer == 0 && seats[seat].state == state.empty: seats[seat].customer != 0);
            return default(bool);
        }
    }


    /// <summary>
    /// Enumerated states for eat seat in a performance
    /// </summary>
    public enum state {
        /// <summary>
        /// Empty is 0 and all seats are set up with the value 0 which allows the theatre to see if it is still empty
        /// </summary>
        empty,
        /// <summary>
        /// once a seat has been reserved the state of the seat will change to reserved
        /// </summary>
        reserved,
        /// <summary>
        /// when the seat is bought its state can then be changed to purchased
        /// </summary>
        purchased,
        /// <summary>
        /// Seats which are not needed for a performance
        /// </summary>
        unused
    };

    /// <summary>
    /// struct to store the seats state and customer associated with it
    /// </summary>
    public struct Seat
    {
        /// <summary>
        /// storing the seat state for a performance
        /// </summary>
        public state state;

        /// <summary>
        /// storing the customer associated with the current seat
        /// </summary.]']>
        public int customer;
    }

    /// <summary>
    /// Implementation class which contains the methods and functions of the theatre
    /// </summary>
    public class Theatre : ITheatre
    {

        /// <summary>
        ///  Invariants for the performance which it must meet in any method
        /// </summary>
        [ContractInvariantMethod]
        void ObjectInvariant()
        {
            //seats available must always be between 0 and 150
            Contract.Invariant(max > 0 && max <= 150);
            Contract.Invariant(available <= max);
            Contract.Invariant(performanceStart ? available >= 0 : available >= (max * safetyNum));
        }

        /// <summary>
        /// constructor to set up theatre for new performance
        /// </summary>
        /// <param name="seatsRequired">Holds the amount of seats needed for a performance</param>
        public Theatre(int seatsRequired)
        {
            //setting the max seats to the required amount
            max = seatsRequired;
            available = max;
            seats = new Seat[capacity];

            // for loop sets all seats which are not needed for a performance to an unused state
            for (int i = max; i < 150; i++)
            {
                seats[i].state = state.unused;
            }

        }

        int capacity = 150;
        int max;
        const double safetyNum = 0.04;
        Seat[] seats;
        int available;
        bool performanceStart = false;

        /// <summary>
        /// returning the value of available to the interface
        /// </summary>
        int ITheatre.available
        {
            get
            {
                return available;
            }
        }
        /// <summary>
        /// returning the seats to the interface
        /// </summary>
        Seat[] ITheatre.seats
        {
            get
            {
                return seats;
            }
        }
        /// <summary>
        /// returning the max number of seats to the interface
        /// </summary>
        int ITheatre.max
        {
            get
            {
                return max;
            }
        }

        /// <summary>
        /// Customer can Buy seat for one performance
        /// </summary>
        /// <param name="customer">Customer id which will be placed into the seat struct</param>
        /// <param name="seat">Seat number which will be placed into the seat struct</param>
        /// <returns>true</returns>
        public bool buySeat(int customer, int seat)
        {
            if (seats[seat].customer == 0 || (seats[seat].customer == customer && seats[seat].state == state.reserved))
            {
                seats[seat].customer = customer;
                seats[seat].state = state.purchased;
                available--;
                return true;
            }
            return false;
        }
        /// <summary>
        /// Remove all reservations for a performance
        /// </summary>
        /// <returns>True</returns>
        public bool cancelReservations()
        {
            for (int i = 0; i < max; i++)
            {
                seats[i].customer = 0;
                seats[i].state = state.empty;
                available++;
                if (available == 150)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// check to see there are any seats left to book
        /// </summary>
        /// <returns>Amount of seats left to purchase/reserve</returns>
        public int checkAvailability()
        {
            return available;
        }
        /// <summary>
        /// check to see if a customer has a seat
        /// </summary>
        /// <param name="seat">seat number that will be checked to see if a customer has bought it</param>
        /// <returns></returns>
        public Seat checkCustomer(int seat)
        {
            return seats[seat];
        }
        /// <summary>
        /// generate a visual for the layout of seats for a performance 
        /// </summary>
        /// <returns>Returns a layout for the seats of a performance</returns>
        public Seat[] printSeatingPlan()
        {
            return seats;
        }
        /// <summary>
        /// when there is only 2 hours left till the performance the reserved seats will become available again
        /// </summary>
        public void removeReserved()
        {
            for (int i = 0; i < max; i++)
            {
                if (seats[i].state == state.reserved)
                {
                    seats[i].state = state.empty;
                    available++;
                }
            }
        }
        /// <summary>
        /// Customer can reserve seat for one performance
        /// </summary>
        /// <param name="customer">Customer number to be stored into the seat struct</param>
        /// <param name="seat">the seat number which stores the customer and state</param>
        /// <returns>True</returns>
        public bool reserveSeat(int customer, int seat)
        {
            state oldValue;
            if (seats[seat].customer.Equals(0) && seats[seat].state == state.empty)
            {
                oldValue = seats[seat].state;
                seats[seat].customer = customer;
                seats[seat].state = state.reserved;
                available--;
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Customer can return seat reservation for one performance
        /// </summary>
        /// <param name="customer">customer value to check seat matches</param>
        /// <param name="seat">seat number that the customer should be assigned to</param>
        /// <returns>True</returns>
        public bool returnSeat(int customer, int seat)
        {
            
            state oldState = seats[seat].state;
            if (seats[seat].customer == customer && seats[seat].state == state.reserved)
            {

                seats[seat].customer = 0;
                seats[seat].state = state.empty;
                available++;
                return true;
            }
            return false;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create a dictonary of the theatre which will hold each performance with an index key
            Dictionary<int, ITheatre> theatre = new Dictionary<int, ITheatre>();
            // Creating two new performances of the theatre and passing the seat allocation
            ITheatre musical = new Theatre(100);
            ITheatre opera = new Theatre(150);
            // Adding each performance into the theatre dictonary
            theatre.Add(1, musical);
            theatre.Add(2, opera);

            Console.WriteLine("musical Seat: {0}", musical.checkAvailability());
            Console.WriteLine("opera Seat: {0}", opera.checkAvailability());
            newPerformance(musical, 100);
            Console.ReadLine();
            newPerformance(opera, 150);
            Console.ReadLine();
        }

        static void newPerformance(ITheatre availability, int max)
        {
            Console.WriteLine("Starting Availability: {0}", availability.checkAvailability());

            Seat seat;
            Seat[] layout;
            int customerSeat = 1;
            Console.WriteLine("35 people reserved a seat each");
            for (int i = 1; i <= 35; i++)
            {
                availability.reserveSeat(i, customerSeat);
                customerSeat++;
            }
            Console.WriteLine("35 people bought a seat each");
            for (int i = 1; i < 35; i += 3)
            {
                availability.buySeat(i, (customerSeat + i));
            }
            if (availability.returnSeat(5,5))
            {
                Console.WriteLine("Customer 5 returned their reserved seat: seat 5");
                Console.WriteLine("availability: {0}", availability.checkAvailability());
            }

            if (availability.buySeat(1, 1))
            {
                Console.WriteLine("Customer 1 bought seat 1 that they reserved");
                Console.WriteLine("availability: {0}", availability.checkAvailability());
            }
            int buySeat = 50;
            for (int i = 50; i <= 60; i++)
            {
                availability.buySeat(i, buySeat);
                buySeat++;
            }
            //availability.cancelReservations();
            seat = (availability.checkCustomer(45));
            Console.WriteLine("Current Availability {0}", availability.checkAvailability());
            Console.WriteLine("Check Customer Seat: {0} | {1}", seat.customer, seat.state);
            Console.ReadLine();

            layout = availability.printSeatingPlan();
            int seatNum = 0;
            for (int i = 0; i < 15; i++)
            {
                    for (int j = 0; j < 10; j++)
                    {
                            Console.Write("{0}:{1} | ", layout[seatNum].customer, layout[seatNum].state);
                            seatNum++;
                    }
                    Console.WriteLine();
            }

            Console.WriteLine("***** 2 hours until performance, all reserved become available ***** ");
            availability.removeReserved();
            Console.WriteLine();
            layout = availability.printSeatingPlan();
            seatNum = 0;
            for (int i = 0; i < 15; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    Console.Write("{0}:{1} | ", layout[seatNum].customer, layout[seatNum].state);
                    seatNum++;
                }
                Console.WriteLine();
            }
        }
    }
}
