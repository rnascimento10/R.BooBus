using System;
using Xunit;
namespace R.BooBus.Tests
{
    public class AzureServiceBusEventTests
    {
        [Theory]
        [InlineData("", "aaaa_1")]
        [InlineData("      ", "aaaa_1")]
        [InlineData(null, "aaaa_1")]
        public void When_instantiate_a_new_event_and_your_topic_is_null_or_empty_should_raise_a_exception(string topic, string subscription)
        {
          
            var ex =  Assert.Throws<ArgumentNullException>(() => new FakeEvent(topic, subscription));

            Assert.Contains("Topic should not be null or empty", ex.Message);
              
        }

        [Theory]
        [InlineData("aaa", "")]
        [InlineData("aaa", "   ")]
        [InlineData("aaa", null)]
        public void When_instantiate_a_new_event_and_your_subscription_is_null_or_empty_should_raise_a_exception(string topic, string subscription)
        {

            var ex = Assert.Throws<ArgumentNullException>(() => new FakeEvent(topic, subscription));

            Assert.Contains("Subscription should not be null or empty", ex.Message);

        }

        [Fact]
        public void When_isntantitate_a_event_id_and_createAt_properties_should_not_be_null() 
        {
            var sut = new FakeEvent("A", "B");

            Assert.NotNull(sut.Id);
            Assert.NotNull(sut.CreateAt);

        }


    }
}
