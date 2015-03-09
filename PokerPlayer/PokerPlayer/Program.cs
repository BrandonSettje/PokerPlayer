using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PokerPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Console.ReadKey();
        }
    }
    class PokerPlayer
    {
        List<Card> DrawHand { get; set; }

        // Enum of different hand types
        public enum HandType
        {
            HighCard,
            OnePair,
            TwoPair,
            ThreeOfAKind,
            Straight,
            Flush,
            FullHouse,
            FourOfAKind,
            StraightFlush,
            RoyalFlush
        }
        // Rank of hand that player holds
        public HandType HandRank
        {
            get
            {
                if (HasRoyalFlush()) { return HandType.RoyalFlush; }
                if (HasStraightFlush()) { return HandType.StraightFlush; }
                if (HasFourOfAKind()) { return HandType.FourOfAKind; }
                if (HasFullHouse()) { return HandType.FullHouse; }
                if (HasFlush()) { return HandType.Flush; }
                if (HasStraight()) { return HandType.Straight; }
                if (HasThreeOfAKind()) { return HandType.ThreeOfAKind; }
                if (HasTwoPair()) { return HandType.TwoPair; }
                if (HasPair()) { return HandType.OnePair; }
                return HandType.HighCard;
            }
        }
        // Constructor that isn't used
        public PokerPlayer() { }
        public void hand(List<Card> cards)
        {
            this.DrawHand = cards;
        }
        public bool HasPair()
        {
            return this.DrawHand.GroupBy(x => x.Rank).Count(x => x.Count()  == 2) == 1;  
        }
        public bool HasTwoPair()
        {
            return this.DrawHand.GroupBy(x => x.Rank).Count(x => x.Count() == 2) == 2;
        }
        public bool HasThreeOfAKind()
        {
            return this.DrawHand.GroupBy(x => x.Rank).Count(x => x.Count() == 3) == 1 && !HasPair();
        }
        public bool HasStraight()
        {
            return (this.DrawHand.OrderBy(x => x.Rank).Last().Rank) - (this.DrawHand.OrderBy(x => x.Rank).First().Rank) == 4; 
        }
        public bool HasFlush()
        {
            return this.DrawHand.GroupBy(x => x.Suit).Count() == 1;
        }
        public bool HasFullHouse()
        {
            return this.DrawHand.GroupBy(x => x.Rank).Count(x => x.Count() == 3) == 1 && HasPair();
        }
        public bool HasFourOfAKind()
        {
            return ((this.DrawHand.GroupBy(x => x.Rank).Count(x => x.Count() == 4) == 1)
                && (this.DrawHand.GroupBy(x => x.Rank).OrderByDescending(x => x.Count()).First().Select(x => x.Suit).Distinct().Count() == 4));
        }
        public bool HasStraightFlush()
        {
            return HasStraight() && HasFlush();
        }
        public bool HasRoyalFlush()
        {
            // to get the Royal Flush to pass the test, I asked if there was 1 ten and 1 ace, and if it met the rules for a straight flush
            if ((this.DrawHand.Where(x => x.Rank.Equals(Rank.Ten)).Count() == 1) && (this.DrawHand.Where(x => x.Rank.Equals(Rank.Ace)).Count() == 1))
            {
                return HasStraightFlush();
            }
            return false;
        }  
    }
    //Guides to pasting your Deck and Card class

    //  *****Deck Class Start*****
    class Deck
    {
        /// <summary>
        /// Gets deck of cards ready for holding
        /// </summary>
        public List<Card> DeckOfCards { get; set; }
        /// <summary>
        /// A pile for discarded cards.
        /// </summary>
        public List<Card> DiscardedCards { get; set; }
        /// <summary>
        /// Builds the deck from the enum.
        /// </summary>
        public Deck()
        {
            //Retrieves the deck pile to build -- the deck of cards
            this.DeckOfCards = new List<Card>();
            //Retrieves the discarded pile
            this.DiscardedCards = new List<Card>();
            //Starts the Suit assignment.
            for (int s = 1; s <= 4; s++)
            {
                //Starts the Rank assignment.
                for (int r = 2; r <= 14; r++)
                {
                    //Adds the card with the Rank and Suit assignment
                    this.DeckOfCards.Add(new Card(r, s));
                }
            }
        }
        /// <summary>
        /// If a game requires multiple decks include this and add number of decks to the deck of cards
        /// </summary>
        /// <param name="multiDeck">user input of number of decks</param>
        public Deck(int multiDeck)
        {
            //Retrieves the deck of cards
            this.DeckOfCards = new List<Card>();
            //Retrieves the discarded pile
            this.DiscardedCards = new List<Card>();
            //Starts the multiple deck countdown
            while (multiDeck > 0)
            {
                for (int s = 1; s <= 4; s++)
                {
                    for (int r = 2; r <= 14; r++)
                    {
                        //Adds a new card with a Suit and Rank
                        this.DeckOfCards.Add(new Card(s, r));
                    }
                }
                //Counts down the deck count
                multiDeck--;
            }
        }
        /// <summary>
        /// Can shuffle the deck
        /// </summary>
        public void Shuffle()
        {
            Random rng = new Random();
            //Adds all the discarded cards back to the deck of cards
            this.DeckOfCards.AddRange(DiscardedCards);
            //Temp list for the shuffle
            List<Card> shuffleIt = new List<Card>();
            //loops through the decks of cards until empty
            while (this.DeckOfCards.Count > 0)
            {
                //grabs a random card from the deck of cards
                Card card = this.DeckOfCards[rng.Next(0, this.DeckOfCards.Count)];
                //puts it in the temp deck
                shuffleIt.Add(card);
                //removes the card from the deck of cards
                this.DeckOfCards.Remove(card);
            }
            //once every card has been put in the temp deck, we turn it back to the deck of cards
            this.DeckOfCards = shuffleIt;
        }
        /// <summary>
        /// Can deal a number of cards
        /// </summary>
        /// <param name="numberOfCards">the number of cards wanted from the deck of cards</param>
        /// <returns>the cards being dealt to the user</returns>
        public List<Card> Deal(int numberOfCards)
        {
            //the cards being dealt temp deck
            List<Card> cardsBeingDealt = new List<Card>();
            //Loops until the usier input is reached
            for (int i = 0; i < numberOfCards; i++)
            {
                //grabs the top card of the deck
                Card drawn = this.DeckOfCards.First();
                //adds the card to the temp deck
                cardsBeingDealt.Add(drawn);
                //removes the card from the deck of cards
                this.DeckOfCards.Remove(drawn);
            }
            //returns the cards -in a temp deck- to the user
            return cardsBeingDealt;
        }
        /// <summary>
        /// Allows a number of cards to be discarded
        /// </summary>
        /// <param name="cards">the number of cards wanting to be discarded</param>
        public void Discard(List<Card> cards)
        {
            //adds all cards directly to the discarded card pile
            this.DiscardedCards.AddRange(cards);
        }
        /// <summary>
        /// can discard a single card
        /// </summary>
        /// <param name="card">the card wanting to be discarded</param>
        public void Discard(Card card)
        {
            //adds the card directly to the dicard pile
            this.DiscardedCards.Add(card);
        }
    }
    //  *****Deck Class End*******

    //  *****Card Class Start*****
    public enum Rank
    {
        Two = 2,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Ten,
        Jack,
        Queen,
        King,
        Ace
    }
    /// <summary>
    /// the enums of the suits for the deck of cards builder
    /// </summary>
    public enum Suit
    {
        Spade = 1,
        Club,
        Heart,
        Diamond
    }
    /// <summary>
    /// describing the card
    /// </summary>
    class Card
    {
        /// <summary>
        /// 
        /// </summary>
        public Rank Rank { get; set; }
        public Suit Suit { get; set; }
        /// <summary>
        /// constructs the card properties of having a suit and rank
        /// </summary>
        /// <param name="cRank">the rank</param>
        /// <param name="cSuit">the suit</param>
        public Card(int cRank, int cSuit)
        {
            //setting the rank 
            this.Rank = (Rank)cRank;
            //setting the suit
            this.Suit = (Suit)cSuit;
        }
        /// <summary>
        /// basic return to the user of what type of card it is 
        /// </summary>
        /// <returns>the suit and rank of the card</returns>
        public string GetCardString()
        {
            return this.Rank + " of " + this.Suit;
        }
    }


    //  *****Card Class End*******
}
