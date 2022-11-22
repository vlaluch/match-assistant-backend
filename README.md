# Match assistant bot
This bot is helpful for counting participants of some meeting or event in group chats.
User need to send + if he/she is ready to participate, - if wan't.
Also you can use "+-" if you are not sure.
Some advanced usages like "+ 2 with me" or "+ John" are possible.
Bot will automatically display current counter after each "count" message.

# Commands
* **Count** ( /count or /c ) shows current count;
* **List** ( /list or /l ) shows full list of participants;
* **New game** ( /new or /n ) starts counting for the new game;
* **Ping** ( /ping or /p ) calls chat users who participated in recent games to write decision about current one;

# Solution
Respository contains console application client and web application.
Console app uses long polling method to get updates for chats.
Web app needs configured webhook.
Both apps use database for storing data.
