/*
** EPITECH PROJECT, 2021
** zappy
** File description:
** action
*/

#include "server.h"

void do_actions(server_t *s, map_t *m)
{
    switch (s->acs->dur) {
        case LOOK:
            look(s, m);
            break;
        case FORWARD:
            forward(s, m);
            break;
        case LEFT:
            left(s);
            break;
        case RIGHT:
            right(s);
            break;
        case INVENTORY:
            request_inventory(s);
            break;
        case TAKE:
            take(s, m);
            break;
        case SET:
            set(s, m);
            break;
        case EJECT:
            eject(s, m);
            break;
        case CONNECT:
            connect_nbr(s);
            break;
        case FORK:
            forked(s);
            break;
        case INCANTATION:
            incantation(s);
            break;
        default:
            break;
    }
}

void actions(server_t *s, map_t *m)
{
    clock_t new;
    action_t *head = s->acs;

    for (; s->acs != NULL; s->acs = s->acs->next) {
        new = clock() - s->acs->begin;
        if (new / CLOCKS_PER_SEC >= get_durations(s->acs->dur) / s->freq) {
            if (s->acs->dur != DEATH && s->acs->dur != REFILL && s->acs->dur != FOOD) {
                do_actions(s, m);
                if (s->acs->next == NULL) {
                    s->acs->prev->next = NULL;
                    free(s->acs);
                } else {
                    s->acs->prev->next = s->acs->next;
                    s->acs->next->prev = s->acs->prev;
                    free(s->acs);
                }
            } else {
                if (s->acs->dur == FOOD) {
                    go_previous(s);
                    for (; s->players->next != NULL; s->players = s->players->next) {
                        if (s->players->type == AI) {
                            s->players->inventory[0] -= 1;
                            if (s->players->inventory[0] <= 0) {
                                dprintf(s->players->fd, "Dead\n");
                                dprintf(s->gui_fd, "pdi %d\n", s->players->pos);
                                s->players->level = 0;
                                close(s->players->fd);
                                s->players->fd = 0;
                                s->players->type = ANY;
                                s->players->x = 0;
                                s->players->y = 0;
                            }
                        }
                    }
                }
            }
        }
    }
    s->acs = head;
}