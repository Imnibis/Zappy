#!/usr/bin/env python3

from sys import argv as av
from sys import stderr, stdout
from sys import exit as xit
from Client import Client
from Map import Map
from Player import Player
from Socket import Socket
from AI import loop
from os import fork

def connection(cli:Client, sock:Socket, map_info:Map, ply:Player) -> None:
    sock.connect(cli.machine, cli.port)
    output = sock.receive(ply)
    if (output == "WELCOME\n"):
        sock.send(cli.name + '\n')
        output = sock.receive(ply)
        splitted = output.split('\n')
        try:
            cli.can_connect = int(splitted[0])
        except ValueError:
            stderr.write("ValueError: bad value given for CLIENT-NUM\n")
            xit(84)
        splitted = splitted[1].split(' ')
        try:
            map_info.msz[0] = int(splitted[0])
            map_info.msz[1] = int(splitted[1])
        except ValueError:
            stderr.write("ValueError: bad value given for TEAM-NAME\n")
            xit(84)
    elif (output == "ko\n"):
        stderr.write("ServerError: any welcome messages ... rude\n")
        sock.sock.close()
        xit(84)

def main() -> None:
    cli = Client()
    ply = Player()
    sock = Socket()
    map_info = Map()
    connection(cli, sock, map_info, ply)
    loop(cli, sock, map_info, ply)

def helper():
    stdout.write("USAGE:\t./zappy_ai -p port -n name -h machine\n"
    "\tport\tis the port number\n\tname\tis the name of the team\n"
    "\tmachine\tis the name of the machine; localhost by default\n")
    xit(0)

def cmd_checkout() -> None:
    if (len(av) < 2 or len(av) > 7):
        stderr.write("Error: too many or not enough arguments, check --help\n")
        xit(84)
    if (av[1] == "--help"):
        helper()

if __name__ == '__main__':
    try:
        cmd_checkout()
        main()
    except KeyboardInterrupt:
        xit(0)