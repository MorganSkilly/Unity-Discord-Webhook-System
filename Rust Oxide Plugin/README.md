# Rust-Oxide-Easy-Discord-Integration
A Rust Oxide plugin for sending webhooks with embedded files to Discord from a Rust server

# Quickstart
- 1: Add the plugin.cs file to your Oxide plugins folder on the server
- 2: Add the config json to the Oxide config folder
- 3: In config add relevent Discord webhooks as needed
- 4: Disable sandbox by creating an empty file in RustDedicated_Data\Managed called "oxide.disable-sandbox"
- 5: Run the server to check webhooks are being used correctly
- 6: Try using the command "/discord test" in game (remember you will need to assign perms to do this.)
- 7: The mod uses Oxide's permission system, assign the permission with "easydiscord.use"
- 8: You can then begin adding more webhook calls by calling the send functions from Oxide hooks and adding more webhooks by adjusting the config file 

Check the other functions below for info

# About
This plugin is based on my Unity Discord script that can be found in the root folder of this repository

# Usage
Feel free to use this mod how ever you like, Please credit me in your server info though.

# Functions
Simple message functions and parameters
  - EasyDiscordIntegration.Send(string mssgBody)
  - EasyDiscordIntegration.Send(string mssgBody, string userName)
  - EasyDiscordIntegration.Send(string mssgBody, string userName, string webhook)
  
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
- note

https://morgan.games/
