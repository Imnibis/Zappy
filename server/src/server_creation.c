/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** server_creation
*/

#include "server.h"

void set_rfd(server_t *s, int *new, fd_set *rfd, fd_set *wfd)
{
    FD_ZERO(rfd);
    FD_ZERO(wfd);
    FD_SET(s->sockid, rfd);
    FD_SET(s->sockid, wfd);
    for (; s->players->next != NULL; s->players = s->players->next) {
        if (s->players->fd > 0)
            FD_SET(s->players->fd, rfd);
        if (s->players->fd > *new)
            *new = s->players->fd;
    }
    go_previous(s);
}

void set_newclient(server_t *s, int *sock)
{
    for (; s->players->next != NULL; s->players = s->players->next) {
        if (s->players->fd == 0) {
            s->players->fd = *sock;
            break;
        }
    }
    s->nb_cli += 1;
    dprintf(*sock, "WELCOME\n");
    go_previous(s);
}

void init_server(server_t *s, map_t *m, server_config_t *si)
{
    int max_fd = s->sockid;
    int nw;
    fd_set rfd;
    fd_set wfd;
    struct sockaddr_in adr;
    socklen_t ads = sizeof(adr);
    char *str = NULL;

    gui_fd = 0;
    init_clients(s);
    while (1) {
        set_rfd(s, &max_fd, &rfd, &wfd);
        if (select(max_fd + 1, &rfd, &wfd, NULL, 0x0) < 0)
            break;
        if (FD_ISSET(s->sockid, &rfd)) {
            if ((nw = accept(s->sockid, (struct sockaddr *)&adr, &ads)) < 0)
                break;
            set_newclient(s, &nw);
        }
        for (; s->players->next != NULL; s->players = s->players->next) {
            if (FD_ISSET(s->players->fd, &rfd)) {
                if ((str = get_next_line(s->players->fd)) == NULL) {
                    close(s->players->fd);
                    s->players->fd = 0;
                    FD_CLR(s->players->fd, &rfd);
                    FD_CLR(s->players->fd, &wfd);
                } else {
                    if (s->players->type != ANY)
                        command_handling(s, str);
                    if (strcmp(str, "GRAPHIC") == 0 && s->players->type == ANY) {
                        s->players->type = CLIENT;
                        gui_fd = s->players->fd;
                        send_map_gui(s, m);
                    }
                    if (team_exists(si, str) == 0 && s->players->type == ANY) {
                        s->players->type = AI;
                        dprintf(s->players->fd, "%d\n%d %d\n", s->cli_max - s->nb_cli, m->width, m->height);
                    }
                    printf("%s\n", str);
                }
            }
        }
        go_previous(s);
        free(str);
    }
}

int serv_attribution(server_t *s)
{
    if (bind(s->sockid, (struct sockaddr *)&s->serv, s->size) < 0) {
        shutdown(s->sockid, SHUT_RDWR);
        close(s->sockid);
        return 1;
    }
    if (listen(s->sockid, s->cli_max) < 0) {
        close(s->sockid);
        return 1;
    }
    return 0;
}

int create_server(server_t *s, server_config_t *info)
{
    if ((s->sockid = socket(AF_INET, SOCK_STREAM, 0)) == -1)
        return 1;
    s->port = info->port;
    s->serv.sin_family = AF_INET;
    s->serv.sin_port = htons(s->port);
    s->serv.sin_addr.s_addr = INADDR_ANY;
    s->size = sizeof(s->serv);
    s->cli_max = info->clientsNb;
    s->nb_cli = 0;

    return 0;
}