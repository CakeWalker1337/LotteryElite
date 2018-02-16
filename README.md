# LotteryElite

## Description
This is test lottery application (just not serious lottery). The same thing with information system, but game :)
Users can buy tickets for different games, based on one of principles "5x26", "6x42", "1x2" and "7x49" (like **100loto**) and try to win.
Prices for win are computing by personal rules:

Piece = Pool * 0.01%
- 1 match = piece
- 2 match = piece x2 (x2 by prev prize)
- 3 match = piece x10 (x5 by prev prize)
- 4 match = piece x20 (x2 by prev prize)
...
- n match = piece x(2*n/2 + 5*(n/2-1))

Admin can make all admin-operations (create/edit/delete) with games, users, tickets, and start game (if game not ends).
Moderator can make all admin-operations except editing permission of users and starting games.
> Note: This is my first project on C#


## Screenshots (sorry for broken eyes)

![alt text](https://raw.github.com/CakeWalker1337/LotteryElite/master/github/screenshots/1.jpg)

Picture 1 - User profile

![alt text](https://raw.github.com/CakeWalker1337/LotteryElite/master/github/screenshots/2.jpg)

Picture 2 - List of games

![alt text](https://raw.github.com/CakeWalker1337/LotteryElite/master/github/screenshots/3.jpg)

Picture 3 - Admin-panel

![alt text](https://raw.github.com/CakeWalker1337/LotteryElite/master/github/screenshots/4.jpg)

Picture 4 - Ticket options (when user is buying ticket)

## Other information

Technologies:
* MySQL
* WPF
