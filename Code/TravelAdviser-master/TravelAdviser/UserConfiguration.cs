using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace TravelAdviser;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    private const int NameMaxLength = 50;
    private const int PasswordMaxLength = 30;
    private const int PhoneMaxLength = 20;
    private const int EmailMaxLength = 100;

    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        //builder.HasKey(e => e.Id);

        //builder.HasIndex(e => e.Email).IsUnique();
        //builder.HasIndex(e=>e.PhoneNumber).IsUnique();  

        //builder.Property(e => e.FirstName).HasMaxLength(NameMaxLength);
        //builder.Property(e => e.LastName).HasMaxLength(NameMaxLength);
        //builder.Property(e => e.Password).HasMaxLength(PasswordMaxLength);
        //builder.Property(e => e.PhoneNumber).HasMaxLength(PhoneMaxLength);
        //builder.Property(e => e.Email).HasMaxLength(EmailMaxLength);

        //builder.HasOne(e => e.Cart)
        //       .WithOne()
        //       .HasForeignKey<Cart>(e => e.UserId)
        //       .HasConstraintName("FK_Cart_User_UserId")
        //       .OnDelete(DeleteBehavior.Cascade);
    }
}
