﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Configuration;

#nullable disable

namespace Server.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20230827134322_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("AliasUser", b =>
                {
                    b.Property<Guid>("AliasesId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("AliasesId", "UsersId");

                    b.HasIndex("UsersId");

                    b.ToTable("UsersAliases", (string)null);
                });

            modelBuilder.Entity("Server.Domain.Alias", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Display")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("EntityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("Group_FK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Source")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Type")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Group_FK");

                    b.ToTable("Aliases");
                });

            modelBuilder.Entity("Server.Domain.Group", b =>
                {
                    b.Property<Guid>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Disabled")
                        .HasColumnType("bit");

                    b.Property<int>("GridDisplay")
                        .HasColumnType("int");

                    b.Property<bool>("IsRepeatCard")
                        .HasColumnType("bit");

                    b.Property<Guid>("Template_FK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("TitleSection")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("GroupId");

                    b.HasIndex("Template_FK");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Server.Domain.Language", b =>
                {
                    b.Property<Guid>("Id_Language")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("NameOfLanguage")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id_Language");

                    b.ToTable("Languages");
                });

            modelBuilder.Entity("Server.Domain.Log_Doc", b =>
                {
                    b.Property<Guid>("Id_Doc")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.HasKey("Id_Doc");

                    b.ToTable("Log_Docs");
                });

            modelBuilder.Entity("Server.Domain.Template", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Language_FK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("Log_Doc_FK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Style")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Language_FK");

                    b.HasIndex("Log_Doc_FK");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("Server.Domain.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserEmail")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AliasUser", b =>
                {
                    b.HasOne("Server.Domain.Alias", null)
                        .WithMany()
                        .HasForeignKey("AliasesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Domain.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Server.Domain.Alias", b =>
                {
                    b.HasOne("Server.Domain.Group", "Group")
                        .WithMany("Aliases")
                        .HasForeignKey("Group_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Server.Domain.TypeSettings", "TypeSetting", b1 =>
                        {
                            b1.Property<Guid>("IdSettings")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("AliasId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<string>("DefaultValue")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("FieldType")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<bool>("ReadOnly")
                                .HasColumnType("bit");

                            b1.Property<bool>("Required")
                                .HasColumnType("bit");

                            b1.HasKey("IdSettings");

                            b1.HasIndex("AliasId")
                                .IsUnique();

                            b1.ToTable("TypeSettings");

                            b1.WithOwner()
                                .HasForeignKey("AliasId");
                        });

                    b.Navigation("Group");

                    b.Navigation("TypeSetting");
                });

            modelBuilder.Entity("Server.Domain.Group", b =>
                {
                    b.HasOne("Server.Domain.Template", "Template")
                        .WithMany("Groups")
                        .HasForeignKey("Template_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");
                });

            modelBuilder.Entity("Server.Domain.Template", b =>
                {
                    b.HasOne("Server.Domain.Language", "Language")
                        .WithMany("Templates")
                        .HasForeignKey("Language_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Server.Domain.Log_Doc", "Log_Doc")
                        .WithMany("Templates")
                        .HasForeignKey("Log_Doc_FK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Language");

                    b.Navigation("Log_Doc");
                });

            modelBuilder.Entity("Server.Domain.Group", b =>
                {
                    b.Navigation("Aliases");
                });

            modelBuilder.Entity("Server.Domain.Language", b =>
                {
                    b.Navigation("Templates");
                });

            modelBuilder.Entity("Server.Domain.Log_Doc", b =>
                {
                    b.Navigation("Templates");
                });

            modelBuilder.Entity("Server.Domain.Template", b =>
                {
                    b.Navigation("Groups");
                });
#pragma warning restore 612, 618
        }
    }
}
