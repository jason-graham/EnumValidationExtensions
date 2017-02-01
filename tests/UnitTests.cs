using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel;

namespace EnumValidationExtensionsTests
{
    [TestClass]
    public class UnitTests
    {
        [Flags]
        private enum IntEnumFlags : int
        {
            Value0 = 0,
            Value1 = 1,
            Value2 = 2,
            Value3 = 4
        }

        private enum IntEnum : int
        {
            Value0 = 0,
            Value1 = 1,
            Value2 = 2,
            Value3 = 3
        }

        [Flags]
        private enum InvalidIntEnumFlags : int
        {
            Value0 = 0,
            Value1 = -1,
            Value2 = -2,
            Value3 = -4
        }

        [Flags]
        private enum UIntEnumFlags : uint
        {
            Value0 = 0,
            Value1 = 1,
            Value2 = 2,
            Value3 = 4
        }

        private enum UIntEnum : uint
        {
            Value0 = 0,
            Value1 = 1,
            Value2 = 2,
            Value3 = 3
        }

        [TestMethod]
        public void IntFlagTests()
        {
            Assert.IsTrue(((IntEnumFlags)0).IsDefined());
            Assert.IsTrue(((IntEnumFlags)1).IsDefined());
            Assert.IsTrue(((IntEnumFlags)2).IsDefined());
            Assert.IsTrue(((IntEnumFlags)3).IsDefined());
            Assert.IsTrue(((IntEnumFlags)4).IsDefined());
            Assert.IsTrue(((IntEnumFlags)5).IsDefined());
            Assert.IsTrue(((IntEnumFlags)6).IsDefined());
            Assert.IsTrue(((IntEnumFlags)7).IsDefined());
            Assert.IsFalse(((IntEnumFlags)8).IsDefined());

            Assert.IsTrue(IntEnumFlags.Value0.IsDefined());
            Assert.IsTrue(IntEnumFlags.Value1.IsDefined());
            Assert.IsTrue(IntEnumFlags.Value2.IsDefined());
            Assert.IsTrue(IntEnumFlags.Value3.IsDefined());
            Assert.IsTrue((IntEnumFlags.Value1 | IntEnumFlags.Value2).IsDefined());
            Assert.IsTrue((IntEnumFlags.Value1 | IntEnumFlags.Value2 | IntEnumFlags.Value3).IsDefined());
        }

        [TestMethod]
        public void UIntFlagTests()
        {
            Assert.IsTrue(((UIntEnumFlags)0).IsDefined());
            Assert.IsTrue(((UIntEnumFlags)1).IsDefined());
            Assert.IsTrue(((UIntEnumFlags)2).IsDefined());
            Assert.IsTrue(((UIntEnumFlags)3).IsDefined());
            Assert.IsTrue(((UIntEnumFlags)4).IsDefined());
            Assert.IsTrue(((UIntEnumFlags)5).IsDefined());
            Assert.IsTrue(((UIntEnumFlags)6).IsDefined());
            Assert.IsTrue(((UIntEnumFlags)7).IsDefined());
            Assert.IsFalse(((UIntEnumFlags)8).IsDefined());

            Assert.IsTrue(UIntEnumFlags.Value0.IsDefined());
            Assert.IsTrue(UIntEnumFlags.Value1.IsDefined());
            Assert.IsTrue(UIntEnumFlags.Value2.IsDefined());
            Assert.IsTrue(UIntEnumFlags.Value3.IsDefined());
            Assert.IsTrue((UIntEnumFlags.Value1 | UIntEnumFlags.Value2).IsDefined());
            Assert.IsTrue((UIntEnumFlags.Value1 | UIntEnumFlags.Value2 | UIntEnumFlags.Value3).IsDefined());
        }

        [TestMethod]
        public void IntEnumTests()
        {
            Assert.IsTrue(((IntEnum)0).IsDefined());
            Assert.IsTrue(((IntEnum)1).IsDefined());
            Assert.IsTrue(((IntEnum)2).IsDefined());
            Assert.IsTrue(((IntEnum)3).IsDefined());
            Assert.IsFalse(((IntEnum)4).IsDefined());

            Assert.IsTrue(IntEnum.Value0.IsDefined());
            Assert.IsTrue(IntEnum.Value1.IsDefined());
            Assert.IsTrue(IntEnum.Value2.IsDefined());
            Assert.IsTrue(IntEnum.Value3.IsDefined());
        }

        [TestMethod]
        public void UIntEnumTests()
        {
            Assert.IsTrue(((UIntEnum)0).IsDefined());
            Assert.IsTrue(((UIntEnum)1).IsDefined());
            Assert.IsTrue(((UIntEnum)2).IsDefined());
            Assert.IsTrue(((UIntEnum)3).IsDefined());
            Assert.IsFalse(((UIntEnum)4).IsDefined());

            Assert.IsTrue(UIntEnum.Value0.IsDefined());
            Assert.IsTrue(UIntEnum.Value1.IsDefined());
            Assert.IsTrue(UIntEnum.Value2.IsDefined());
            Assert.IsTrue(UIntEnum.Value3.IsDefined());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidEnumArgumentException))]
        public void InvalidEnumTest()
        {
            ((IntEnum)int.MaxValue).ThrowIfInvalidEnumValue();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void OutOfRangeEnumValueTest()
        {
            ((IntEnumFlags)int.MinValue).IsDefined();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void InvalidEnumValueDefinedTest()
        {
            InvalidIntEnumFlags.Value0.IsDefined();
        }
    }
}
