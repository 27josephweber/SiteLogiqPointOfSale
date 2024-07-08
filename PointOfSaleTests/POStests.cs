using SiteLogiqPointOfSale;

namespace PointOfSaleTests
{
    [TestClass]
    public class POStests
    {
        [TestMethod]
        public void TestBasicOrderFlow()
        {
            PointOfSale myPos = new PointOfSale();
            myPos.AddItem("hot dog", "small"); //5.00
            myPos.AddItem("hamburger", "medium"); //8.50
            myPos.AddItem("fries", "large"); //3.00
            myPos.AddItem("italian beef", "small"); //9.00
            myPos.AddItem("gyro", "medium"); //12.50
            Assert.AreEqual(38.00,myPos.OrderTotal);

            myPos.AddCondiment("ketchup");
            myPos.AddCondiment("mustard");
            Assert.AreEqual(38.50, myPos.OrderTotal);

            myPos.AddCash(20.00);
            //make sure warning message is thrown if cash is insufficient
            try
            {

            } catch (InvalidOperationException ex) { 
                Assert.AreEqual($"Current cash,{myPos.Cash}, is insufficient for this order of {myPos.OrderTotal}" , ex.Message);
            }
            
            myPos.AddCash(10.00);
            myPos.AddCash(10.00);

            CompletedOrder completedOrder = myPos.Serve();

            //check that served items matchup
            Assert.AreEqual(5,completedOrder.OrderItems.Count);
            Assert.AreEqual(2,completedOrder.OrderCondiments.Count);

            //check current POS state
            Assert.AreEqual(0,myPos.OrderItems.Count);
            Assert.AreEqual(0,myPos.OrderCondiments.Count);
            Assert.AreEqual(1.50, myPos.Cash);

            //return change
            double change = myPos.EndTransaction();
            Assert.AreEqual(1.50, change);
            Assert.AreEqual(0, myPos.Cash);


        }
        [TestMethod]
        public void TestItemSizePrices() 
        {
            PointOfSale myPos = new PointOfSale();
            myPos.AddItem("hamburger", "small"); //base price is $8.00
            Assert.AreEqual(8.00, myPos.OrderTotal);
            myPos.EndTransaction();
            myPos.AddItem("hamburger", "medium");
            Assert.AreEqual(8.50, myPos.OrderTotal);
            myPos.EndTransaction();
            myPos.AddItem("hamburger", "large");
            Assert.AreEqual(9.00, myPos.OrderTotal);

            Assert.ThrowsException<ArgumentException>(() => myPos.AddItem("hamburger","extra-large")); //xl is not a valid size

        }
        [TestMethod]
        public void TestAddingCondiments() 
        {
            PointOfSale myPos = new PointOfSale();
            myPos.AddCondiment("ketchup");
            Assert.AreEqual(0.25, myPos.OrderTotal);
            myPos.AddCondiment("mustard");
            Assert.IsTrue(myPos.OrderCondiments.Contains("ketchup"));
            Assert.IsTrue(myPos.OrderCondiments.Contains("mustard"));
            myPos.AddCondiment("ketchup");
            myPos.AddCondiment("ketchup");
            Assert.AreEqual(1.00, myPos.OrderTotal);

            //based on specifiaction, should not be able to add more than 3 of the same condiment
            Assert.ThrowsException<InvalidOperationException>(() => myPos.AddCondiment("ketchup"));
            
            //truffles is not on the condiment list
            Assert.ThrowsException<ArgumentException>(() => myPos.AddCondiment("truffles")); 


        }
        [TestMethod]
        public void TestEndingTransaction() 
        {
            PointOfSale myPos = new PointOfSale();
            myPos.AddItem("hamburger", "small"); //8.00
            myPos.AddItem("hot dog", "medium"); //5.50
            myPos.AddItem("fries", "large");//3.00
            Assert.AreEqual(16.50, myPos.OrderTotal);
            myPos.AddCash(20.00);
            myPos.AddCash(5.00);
            Assert.AreEqual(25.00, myPos.Cash);

            double change = myPos.EndTransaction();

            Assert.AreEqual(25.00, change);
            Assert.AreEqual(0.0, myPos.OrderTotal);
            Assert.AreEqual(0.0, myPos.Cash);
            Assert.AreEqual(0, myPos.OrderItems.Count);
        }
        [TestMethod]
        public void TestAddingCash() {
            PointOfSale myPos = new PointOfSale();
            myPos.AddCash(20.00);
            myPos.AddCash(5.00);
            myPos.AddCash(0.05);
            Assert.AreEqual(25.05, myPos.Cash);

            //$15 bills are not accepted
            Assert.ThrowsException<ArgumentException>(() => myPos.AddCash(15.00));

        }
    }
}