﻿using System.Text;

namespace Clean.Architecture.SharedKernel.Utils;

public static class PasswordGenerator
{
  /// <summary>
  /// Generates a random password based on the rules passed in the parameters
  /// </summary>
  /// <param name="includeLowercase">Bool to say if lowercase are required</param>
  /// <param name="includeUppercase">Bool to say if uppercase are required</param>
  /// <param name="includeNumeric">Bool to say if numerics are required</param>
  /// <param name="includeSpecial">Bool to say if special characters are required</param>
  /// <param name="includeSpaces">Bool to say if spaces are required</param>
  /// <param name="lengthOfPassword">Length of password required. Should be between 8 and 128</param>
  /// <returns></returns>
  public static string GeneratePassword(bool includeLowercase, bool includeUppercase, bool includeNumeric, bool includeSpecial, bool includeSpaces, int lengthOfPassword)
  {
    const int MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS = 2;
    const string LOWERCASE_CHARACTERS = "abcdefghijklmnopqrstuvwxyz";
    const string UPPERCASE_CHARACTERS = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    const string NUMERIC_CHARACTERS = "0123456789";
    const string SPECIAL_CHARACTERS = @"!#$%&*@\";
    const char SPACE_CHARACTER = ' ';

    string combinedPasswordCharacterSet = "";
    StringBuilder compulsoryPasswordCharacters = new StringBuilder();

    if (includeLowercase)
    {
      int index = Random.Shared.Next(LOWERCASE_CHARACTERS.Length);
      compulsoryPasswordCharacters.Append(LOWERCASE_CHARACTERS[index]);

      combinedPasswordCharacterSet += LOWERCASE_CHARACTERS;
    }

    if (includeUppercase)
    {
      int index = Random.Shared.Next(UPPERCASE_CHARACTERS.Length);
      compulsoryPasswordCharacters.Append(UPPERCASE_CHARACTERS[index]);

      combinedPasswordCharacterSet += UPPERCASE_CHARACTERS;
    }

    if (includeNumeric)
    {
      int index = Random.Shared.Next(NUMERIC_CHARACTERS.Length);
      compulsoryPasswordCharacters.Append(NUMERIC_CHARACTERS[index]);

      combinedPasswordCharacterSet += NUMERIC_CHARACTERS;
    }

    if (includeSpecial)
    {
      int index = Random.Shared.Next(SPECIAL_CHARACTERS.Length);
      compulsoryPasswordCharacters.Append(SPECIAL_CHARACTERS[index]);

      combinedPasswordCharacterSet += SPECIAL_CHARACTERS;
    }

    if (includeSpaces)
    {
      compulsoryPasswordCharacters.Append(SPACE_CHARACTER);

      combinedPasswordCharacterSet += SPACE_CHARACTER;
    }

    //we have already fulfilled our required characters, now we just fill up remaining length.
    char[] password = new char[lengthOfPassword];
    int characterSetLength = combinedPasswordCharacterSet.Length;

    for (int characterPosition = 0; characterPosition < lengthOfPassword; characterPosition++)
    {
      password[characterPosition] = combinedPasswordCharacterSet[Random.Shared.Next(characterSetLength - 1)];

      bool moreThanTwoIdenticalInARow =
          characterPosition > MAXIMUM_IDENTICAL_CONSECUTIVE_CHARS
          && password[characterPosition] == password[characterPosition - 1]
          && password[characterPosition - 1] == password[characterPosition - 2];

      if (moreThanTwoIdenticalInARow)
      {
        characterPosition--;
      }
    }

    string generatedPassword = string.Join(null, password);
    compulsoryPasswordCharacters.Append(generatedPassword);

    var finalPassword = compulsoryPasswordCharacters.ToString();

    return finalPassword[..lengthOfPassword];
  }
}