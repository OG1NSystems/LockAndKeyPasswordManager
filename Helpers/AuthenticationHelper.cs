using System;

namespace LockAndKey.Helpers
{
    public class AuthenticationHelper
    {
        public static String GetLockOutTimeRemainingString(DateTime lockedOutDate, Int64 lockedOutCount)
        {
            String timeRemainingString = String.Empty;
            switch (lockedOutCount)
            {
                case 1:
                    if (lockedOutDate.AddMinutes(15) > DateTime.UtcNow)
                    {
                        var timeRemaining = (lockedOutDate.AddMinutes(15) - DateTime.UtcNow).Minutes;
                        if (timeRemaining > 1)
                        {
                            timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinutes}";
                        }
                        else
                        {
                            timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinute}";
                        }
                    }
                    break;
                case 2:
                    if (lockedOutDate.AddMinutes(30) > DateTime.UtcNow)
                    {
                        var timeRemaining = (lockedOutDate.AddMinutes(30) - DateTime.UtcNow).Minutes;
                        if (timeRemaining > 1)
                        {
                            timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinutes}";
                        }
                        else
                        {
                            timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinute}";
                        }
                    }
                    break;
                case 3:
                    if (lockedOutDate.AddHours(1) > DateTime.UtcNow)
                    {
                        var timeRemaining = (lockedOutDate.AddHours(1) - DateTime.UtcNow).Minutes;
                        if (timeRemaining > 1)
                        {
                            timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinutes}";
                        }
                        else
                        {
                            timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinute}";
                        }
                    }
                    break;
                case 4:
                    if (lockedOutDate.AddHours(4) > DateTime.UtcNow)
                    {
                        var timeRemaining = (lockedOutDate.AddHours(4) - DateTime.UtcNow).Minutes;
                        var timeRemainingHours = (lockedOutDate.AddHours(4) - DateTime.UtcNow).Hours;
                        if (timeRemaining > 1 || timeRemainingHours > 0)
                        {
                            if (timeRemainingHours > 0)
                            {
                                while (timeRemaining > 60)
                                {
                                    timeRemaining -= 60;
                                }
                                if (timeRemainingHours > 1)
                                {
                                    timeRemainingString = $"{timeRemainingHours} {Constants.LockOutTimeHours} {timeRemaining} {Constants.LockOutTimeMinutes}";
                                }
                                else
                                {
                                    timeRemainingString = $"{timeRemainingHours} {Constants.LockOutTimeHour} {timeRemaining} {Constants.LockOutTimeMinutes}";
                                }
                            }
                            else
                            {
                                timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinutes}";
                            }
                        }
                        else
                        {
                            timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinute}";
                        }
                    }
                    break;
                case 5:
                    if (lockedOutDate.AddDays(1) > DateTime.UtcNow)
                    {
                        var timeRemaining = (lockedOutDate.AddDays(1) - DateTime.UtcNow).Minutes;
                        var timeRemainingHours = (lockedOutDate.AddDays(1) - DateTime.UtcNow).Hours;
                        if (timeRemaining > 1 || timeRemainingHours > 0)
                        {
                            if (timeRemainingHours > 0)
                            {
                                while (timeRemaining > 60)
                                {
                                    timeRemaining -= 60;
                                }
                                if (timeRemainingHours > 1)
                                {
                                    timeRemainingString = $"{timeRemainingHours} {Constants.LockOutTimeHours} {timeRemaining} {Constants.LockOutTimeMinutes}";
                                }
                                else
                                {
                                    timeRemainingString = $"{timeRemainingHours} {Constants.LockOutTimeHour} {timeRemaining} {Constants.LockOutTimeMinutes}";
                                }
                            }
                            else
                            {
                                timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinutes}";
                            }
                        }
                        else
                        {
                            timeRemainingString = $"{timeRemaining} {Constants.LockOutTimeMinute}";
                        }
                    }
                    break;
            }
            return timeRemainingString;
        }

        public static String SetLockOutMessage(Int64 lockedOutCount)
        {
            String lockOutMessage = String.Empty;
            switch (lockedOutCount)
            {
                case 1:
                    lockOutMessage = String.Format(Constants.LoginMessageLockedOutTime, 15, Constants.LockOutTimeMinutes);
                    break;
                case 2:
                    lockOutMessage = String.Format(Constants.LoginMessageLockedOutTime, 30, Constants.LockOutTimeMinutes);
                    break;
                case 3:
                    lockOutMessage = String.Format(Constants.LoginMessageLockedOutTime, 1, Constants.LockOutTimeHour);
                    break;
                case 4:
                    lockOutMessage = String.Format(Constants.LoginMessageLockedOutTime, 4, Constants.LockOutTimeHours);
                    break;
                case 5:
                    lockOutMessage = String.Format(Constants.LoginMessageLockedOutTime, 1, Constants.LockOutTimeDay);
                    break;

            }
            return lockOutMessage;
        }

        public static Boolean CheckSecurityStringMeetsRequirements(String securityString)
        {
            var containsNumber = false;
            var containsSpecial = false;
            var containsUpper = false;
            var containsLower = false;
            var passwordCharArray = securityString.ToCharArray();
            for (int i = 0; i < passwordCharArray.Length; i++)
            {
                if (!Char.IsLetter(passwordCharArray[i]))
                {
                    if (Char.IsDigit(passwordCharArray[i]))
                    {
                        containsNumber = true;
                    }
                    else
                    {
                        containsSpecial = true;
                    }
                }
                else
                {
                    if (Char.IsUpper(passwordCharArray[i]))
                    {
                        containsUpper = true;
                    }
                    else
                    {
                        containsLower = true;
                    }
                }
            }
            if (containsNumber && containsSpecial && containsUpper && containsLower)
            {
                return true;
            }
            return false;
        }
    }
}
