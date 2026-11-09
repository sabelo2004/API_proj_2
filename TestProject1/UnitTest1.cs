using System.Linq;
using Microsoft.EntityFrameworkCore;
using Xunit;
using API_proj_2.Data;
using API_proj_2.Models;

namespace TestProject1
{
    /// <summary>
    /// Lightweight model-level tests that inspect the EF Core model metadata.
    /// These tests do not hit a real database; they validate indexes and FK configuration
    /// declared in ApplicationDBContext.OnModelCreating().
    /// </summary>
    public class UnitTest1
    {
        [Fact]
        public void Model_Has_Unique_Index_On_User_Email()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase("ModelTest_EmailIndex")
                .Options;

            using var context = new ApplicationDBContext(options);

            var userEntity = context.Model.FindEntityType(typeof(User));
            Assert.NotNull(userEntity);

            var emailIndex = userEntity.GetIndexes()
                .FirstOrDefault(i => i.Properties.Any(p => p.Name == nameof(User.Email)));

            Assert.NotNull(emailIndex);
            Assert.True(emailIndex.IsUnique);
        }

        [Fact]
        public void Model_Has_Unique_Index_On_EventEnrolment_BibNumber()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase("ModelTest_BibIndex")
                .Options;

            using var context = new ApplicationDBContext(options);

            var enrolEntity = context.Model.FindEntityType(typeof(EventEnrolment));
            Assert.NotNull(enrolEntity);

            var bibIndex = enrolEntity.GetIndexes()
                .FirstOrDefault(i => i.Properties.Any(p => p.Name == nameof(EventEnrolment.BibNumber)));

            Assert.NotNull(bibIndex);
            Assert.True(bibIndex.IsUnique);
        }

        [Fact]
        public void Result_Has_ForeignKey_To_EventEnrolment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase("ModelTest_ResultFk")
                .Options;

            using var context = new ApplicationDBContext(options);

            var resultEntity = context.Model.FindEntityType(typeof(Result));
            Assert.NotNull(resultEntity);

            var fk = resultEntity.GetForeignKeys()
                .FirstOrDefault(f => f.PrincipalEntityType.ClrType == typeof(EventEnrolment));

            Assert.NotNull(fk);
            Assert.Contains(fk.Properties, p => p.Name == nameof(Result.EnrolmentID));
        }
    }
}