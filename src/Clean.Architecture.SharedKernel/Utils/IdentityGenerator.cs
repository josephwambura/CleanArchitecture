namespace Clean.Architecture.SharedKernel.Utils;

/// <summary>
/// GUID's byte #3 is the least significant and GUID's byte #10 is the most significant [from SQL Server ORDER BY clause perspective].
/// http://developmenttips.blogspot.com/2008/03/generate-sequential-guids-for-sql.html
/// http://sqlblog.com/blogs/alberto_ferrari/archive/2007/08/31/how-are-guids-sorted-by-sql-server.aspx
/// http://blogs.msdn.com/b/sqlprogrammability/archive/2006/11/06/how-are-guids-compared-in-sql-server-2005.aspx
/// </summary>
public static class IdentityGenerator
{
  private static readonly object syncLock = new object(); // use a private object for locking

  /// <summary>
  ///   This algorithm generates sequential GUIDs across system boundaries, ideal for databases
  /// </summary>
  /// <returns> </returns>
  public static Guid NewSequentialGuid()
  {
    lock (syncLock) // use a system-wide lock
    {
      var uid = Guid.NewGuid().ToByteArray();
      var binDate = BitConverter.GetBytes(DateTime.UtcNow.Ticks);

      // swap the order of bytes for SQL Server compatibility
      Array.Reverse(binDate);

      // copy the date bytes to uid array
      Array.Copy(binDate, 0, uid, 10, 6);

      // set the variant bits according to RFC 4122
      uid[8] = (byte)((uid[8] & 0x3f) | 0x80);

      // set the version bits to indicate time-based UUID
      uid[7] = (byte)((uid[7] & 0x0f) | 0x10);

      return new Guid(uid);
    }
  }
}
