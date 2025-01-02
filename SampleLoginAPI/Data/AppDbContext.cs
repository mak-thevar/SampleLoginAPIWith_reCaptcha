﻿using Microsoft.EntityFrameworkCore;
using SampleLoginAPI.Models;

namespace SampleLoginAPI.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
}
