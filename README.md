# Unity-Discord-Webhook-System
A simple script for sending webhooks with embedded files to Discord from Unity

# Quickstart
- 1: Simply add this script to a unity project
- 2: Change the values in the default static strings at the top of the script to the appropriate vaules for your use case
- 3: Test setup by clicking Tools/Discord/Test
- 4: Begin calling functions from other scripts such as Discord.Send("Hello world!")

Check the other functions below for info

# About
I created this script primarily to send crash reports and analytics back to me on Discord so I can improve my Unity games with data collection.
The script was created with reference to http://www.briangrinstead.com/blog/multipart-form-post-in-c a great blog post showing the basics of how to use multi form post requests.

# Usage
Feel free to use this script how ever you like, It is easy to impliment and allows you to instantly start tracking client data easily. It can also be used to add notifications etc. for players but I recommend only calling the functions in menus etc. to avoid slowing down the game or causing it to hang.

# Functions
Simple message functions and parameters
  - Discord.Send(string mssgBody)
  - Discord.Send(string mssgBody, string userName)
  - Discord.Send(string mssgBody, string userName, string webhook)
  
File sharing message functions and parameters
  - SendFile(
        string mssgBody,
        string filename,
        string fileformat,
        string filepath,
        string application)
        
  - SendFile(
        string mssgBody,
        string filename,
        string fileformat,
        string filepath,
        string application,
        string userName)
        
  - SendFile(
        string mssgBody,
        string filename,
        string fileformat,
        string filepath,
        string application,
        string userName,
        string webhook)
                    
# Notes
- When sending data files I recommend setting the application string to "application/msexcel" in order to default it to open in excel
- File format does not include the dot example "csv" or "txt"
- File path is the fully qualified file path
- File name includes file type example "file.csv" or "data.txt"
- Remember discord will rate limit you if you send too many requests
- IMPORTANT! in order to prevent your game from hanging I recommend you call the script functions as tasks like this, remembering to declare the namespace being used:

```
Task.Factory.StartNew(() => Send("Test"));
```

https://morgan.games/
