using RazorClassLib.Data;
using System.Net.Http;
using System.Net.Http.Json;
using FluentAssertions;
using WebApp.Exceptions;

namespace Test;

public class UnitTest2
{
    [Fact]
    public void AddingTwoNumbers()
    {
        int num1 = 5;
        int num2 = 5;

        int num3 = num1 + num2;

        num3.Should().Be(10);
    }
}