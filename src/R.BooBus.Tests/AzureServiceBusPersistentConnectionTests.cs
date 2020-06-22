using FluentAssertions;
using Microsoft.Azure.ServiceBus;
using NSubstitute;
using Xunit;

namespace R.BooBus.Tests
{
    public class AzureServiceBusPersistentConnecionTests
    {
     

        [Fact]
        public void When_get_a_topic_and_it_is_closed_or_closing_then_a_new_topic_intance_should_be_created()
        {
            //Arrange
            var cn = new ServiceBusConnectionStringBuilder("Endpoint=sb://yourservicebusservice.servicebus.windows.net/;SharedAccessKeyName=Root;SharedAccessKey=yourtopicpolicykey=;EntityPath=fakeentitypath");
            var sut = new FakePersistentConnection(cn);
            var oldInstance = Substitute.For<ITopicClient>();
            oldInstance.IsClosedOrClosing.Returns(true);
            sut.ServiceBusTopicClient = oldInstance;

            //Act
            var newInstance = sut.GetModel();

            //Assert
            oldInstance.Should().NotBeEquivalentTo(newInstance);

            oldInstance.IsClosedOrClosing.Should().BeTrue();
            newInstance.IsClosedOrClosing.Should().BeFalse();
        }

        [Fact]
        public void Should_get_a_same_topic_instance_if_does_not_is_closed_or_closing()
        {
            //Arrange
            var cn = new ServiceBusConnectionStringBuilder("Endpoint=sb://yourservicebusservice.servicebus.windows.net/;SharedAccessKeyName=Root;SharedAccessKey=yourtopicpolicykey=;EntityPath=fakeentitypath");
            var sut = new FakePersistentConnection(cn);
            var instance = Substitute.For<ITopicClient>();
            instance.IsClosedOrClosing.Returns(false);
            sut.ServiceBusTopicClient = instance;

            //Act
            var actual = sut.GetModel();

            //Assert
            instance.Should().BeEquivalentTo(actual);
            instance.IsClosedOrClosing.Should().BeFalse();
            actual.IsClosedOrClosing.Should().BeFalse();
        }


    }
}
