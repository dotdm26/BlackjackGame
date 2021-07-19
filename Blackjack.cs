using System;

namespace BlackjackCsTest
{
    //Ace = 1 so the following values can be equal to their "index" value.
    //The index is the same as their value that is automatically assigned based on Ace = 1's value.
    //e.g. if you obtain Three the index will be 3.
    //Jack, Queen and King's indexes are respectively 11, 12 and 13.
    enum Faces
    {
        Ace = 1,
        Two,
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
        King
    }
    enum Suits 
    {
        Spade, 
        Club, 
        Diamond, 
        Heart };
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string choice;
            Console.WriteLine("===========================================");
            Console.WriteLine("$                BLACKJACK                $");
            Console.WriteLine("===========================================");
            Console.WriteLine("Created by @dotdm26 - Thai Duc Minh Do.\n");

            Console.Write("Would you like to play? (y/n) ");
            //executes first to start the game
            do
            {
                choice = Console.ReadLine();
                switch (choice)
                {
                    case "y":
                        Player player = new Player(1000, 0);
                        Player dealer = new Player(1000, 0);
                        //Loops the game as long as the user wants to keep playing.
                        while (choice != "n")
                        {
                            Game.Play(player, dealer);
                            //Ends the game if a player no longer has money
                            if (MoneyCheckWinner(player, dealer) == true) 
                            { 
                                break; 
                            }
                            else
                            {
                                Console.Write("\nWould you like to go for another round? (y/n) ");
                                choice = Console.ReadLine();
                                if (choice == "n")
                                {
                                    break;
                                }
                                else if (choice != "y")
                                {
                                    while (choice != "y" || choice != "n")
                                    {
                                        Console.Write("Enter a valid option! (y/n) ");
                                        choice = Console.ReadLine();
                                        if (choice == "y" || choice == "n")
                                        {
                                            break;
                                        }
                                    }
                                    continue;
                                }
                            }

                        }
                        Console.WriteLine("\nYou have finished playing the game.");
                        break;
                    case "n":
                        Console.WriteLine("\nYou have opted to not play the game.\n");
                        break;
                    default:
                        Console.Write("Enter a valid option! (y/n) ");
                        continue;
                }
                break;
            } while (choice != "n");
            Console.WriteLine("Goodbye! See you again.");
        }

        static bool MoneyCheckWinner(Player player, Player dealer)
        {
            if (player.money <= 0 || dealer.money <= 0)
            {
                Console.WriteLine("One of the players has no money left to play. The game is over.");
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    class Card
    {
        public string suit, face;
        public int faceIndex, value;
    }

    class Player
    {
        public int money, score, bet, roundsPlayed, roundsWon;
        public Player(int playerMoney, int cardScore)
        {
            money = playerMoney;
            score = cardScore;
        }
    }

    class Game
    {
        public static void Play(Player player, Player dealer)
        {
            string choice = "y";
            int moneyBetSum;
            //reset score after every game loop
            player.score = 0; 
            dealer.score = 0;

            Console.WriteLine("\nLet's play!\n");

            moneyBetSum = MoneyBet(player, dealer);

            //get the dealer's score. Used randomizer for simplicity, though I may change it to something more intricate if the need arises.
            Random rnd = new Random();
            dealer.score = rnd.Next(15, 21);

            //should I try to turn this whole section into a function? Or divide it into two functions?
            while (player.score <= 21 || choice != "n")
            {
                Console.WriteLine("\nDealing your card...");
                Card pickedCard = new Card();
                CardPick(pickedCard);
                Console.WriteLine("Your card is: {0} {1} (+{2})", pickedCard.suit, pickedCard.face, CardValue(player, pickedCard));

                player.score += CardValue(player, pickedCard);
                Console.WriteLine("Your current score is: {0}", player.score);
                if (player.score > 21)
                {
                    break; //ends game if player's score exceeds 21
                }
                else
                {
                    Console.WriteLine("Keep dealing? (y/n)");
                    choice = Console.ReadLine();
                    switch (choice)
                    {
                        case "y":
                            continue;
                        case "n":
                            Console.WriteLine("Ending the game.");
                            break;
                        default:
                            while (choice != "y" || choice != "n")
                            {
                                Console.WriteLine("Enter a valid option! (y/n)");
                                choice = Console.ReadLine();
                                if (choice == "y" || choice == "n")
                                {
                                    break;
                                }
                            }
                            continue;
                    }
                    //completely break out of do-while loop
                    if (choice == "n")
                    {
                       break;
                    }
                }
            }
            GameScore(player, dealer);
            MoneyGiver(player, dealer, moneyBetSum);

            Console.WriteLine("You have won {0} rounds out of {1}.", player.roundsWon, player.roundsPlayed);
        }

        static void CardPick(Card card)
        {
            //NOTE: faceIndex is equal to the value of the face.
            //e.g if faceIndex = 5, the face is Five.
            Random rnd = new Random();
            card.faceIndex = rnd.Next(1, Enum.GetNames(typeof(Faces)).Length);
            int suitIndex = rnd.Next(1, Enum.GetNames(typeof(Suits)).Length);

            card.face = Enum.GetName(typeof(Faces), card.faceIndex);
            card.suit = Enum.GetName(typeof(Suits), suitIndex);
        }

        static int CardValue(Player player, Card card)
        {
            //jack - king. 3 cards, index 11 - 13
            if (card.faceIndex >= 11 && card.faceIndex <= 13)
            {
                card.value = 10;
            }
            //ace card gimmick. +1 if score > 11, +11 if score <= 11
            //NOTE: Find a way where if we have more than 2 Ace cards, their value become 1. If that's how blackjack works.
            else if (card.faceIndex == 1)
            {
                if (player.score > 11)
                {
                    card.value = 1;
                }
                else if (player.score <= 11)
                {
                    card.value = 11;
                }
            }
            //two - ten cards.
            else
            {
                card.value = card.faceIndex;
            }
            return card.value;
        }

        static void GameScore(Player player, Player dealer)
        {
            Console.WriteLine("You scored {0}. The dealer scored {1}.", player.score, dealer.score);
            if (player.score <= 21 && player.score > dealer.score)
            {
                Console.WriteLine("You win!");
                player.roundsWon += 1;
            }
            else if (player.score > 21 || player.score < dealer.score)
            {
                Console.WriteLine("You lose...");
            }
            else
            {
                Console.WriteLine("It's a draw.");
            }
            player.roundsPlayed += 1;
        }

        //consider shortening/dividing this method
        static int MoneyBet(Player player, Player dealer)
        {
            //add betting money
            int sum = 0;
            string input;
            Console.WriteLine("You have ${0} while the dealer has ${1}.", player.money, dealer.money);
            do
            {
                Console.WriteLine("How much would you like to bet?");
                //Verify that the user entered values in the correct format (numbers)
                do
                {
                    input = Console.ReadLine();
                    if (!int.TryParse(input, out player.bet))
                    {
                        Console.WriteLine("Please enter numbers!");
                    }
                } while (!int.TryParse(input, out player.bet));
                player.bet = Convert.ToInt32(input);
                if (player.bet > player.money)
                {
                    Console.WriteLine("You cannot bet more than you have!");
                }
                else if (player.bet <= 0)
                {

                    Console.WriteLine("You cannot bet nothing at all!");
                }
                else if (player.bet > dealer.money)
                {
                    Console.WriteLine("The dealer doesn't have enough money to match your bet. Please lower your bet.");
                }
                continue;
            } while (player.bet > player.money || player.bet <= 0 || player.bet > dealer.money);
            Console.WriteLine("You have put in ${0}", player.bet);
            dealer.bet = player.bet;
            sum = player.bet + dealer.bet;
            player.money -= player.bet;
            dealer.money -= dealer.bet;
            return sum;
        }

        static void MoneyGiver(Player player, Player dealer, int betSum)
        {
            if (player.score > dealer.score && player.score <= 21)
            {
                player.money += betSum;
                Console.WriteLine("You have won ${0}.", betSum);
            }
            else if (player.score < dealer.score || player.score > 21)
            {
                dealer.money += betSum;
                Console.WriteLine("You have lost ${0}.", betSum/2);
            }
            else if (player.score == dealer.score)
            {
                player.money += (betSum / 2);
                dealer.money += (betSum / 2);
                Console.WriteLine("You have drawn.");
            }
            Console.WriteLine("Current amount of money: ${0}", player.money);
        }
    }
}
