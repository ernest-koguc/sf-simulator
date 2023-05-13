import {
  trigger, state, style, transition,
  animate, group
} from '@angular/animations';

export const SlideInOutHorizontallyAnimation = [
  trigger('slideInOutX', [
    state('in', style({
      'opacity': '1', 'visibility': 'visible'
    })),
    state('out', style({
      'opacity': '0', 'visibility': 'hidden', 'transform': 'translateX(100%)'
    })),
    transition('in => out', [group([
      animate('400ms ease-in-out', style({
        'opacity': '0'
      })),
      animate('600ms ease-in-out', style({
        'transform': 'translateX(100%)'
      })),
      animate('700ms ease-in-out', style({
        'visibility': 'hidden'
      }))
    ]
    )]),
    transition('out => in', [group([
      animate('1ms ease-in-out', style({
        'visibility': 'visible'
      })),
      animate('300ms ease-in-out', style({
        'transform': 'translateX(0%)'
      })),
      animate('500ms ease-in-out', style({
        'opacity': '1'
      }))
    ]
    )])
  ]),
]

export const SlideInOutVericallyAnimation = [
  trigger('slideInOutY', [
    state('in', style({
      'opacity': '1', 'visibility': 'visible'
    })),
    state('out', style({
      'opacity': '0', 'visibility': 'hidden', 'transform': 'translateY(100%)'
    })),
    transition('in => out', [group([
      animate('400ms ease-in-out', style({
        'opacity': '0'
      })),
      animate('600ms ease-in-out', style({
        'transform': 'translateY(100%)'
      })),
      animate('700ms ease-in-out', style({
        'visibility': 'hidden'
      }))
    ]
    )]),
    transition('out => in', [group([
      animate('100ms ease-in-out', style({
        'visibility': 'visible'
      })),
      animate('400ms ease-in-out', style({
        'transform': 'translateY(0%)'
      })),
      animate('100ms ease-in-out', style({
        'opacity': '1'
      }))
    ]
    )])
  ]),
];
export const ngIfVerticalSlide =
  trigger(
    'ngIfVerticalSlide',
    [
      transition(
        ':enter',
        [
          style({ transform: "translateX(-100%)", opacity: 0 }),
          animate('300ms ease-out',
            style({ transform: "translateX(0)", opacity: 1 }))
        ]
      ),
      transition(
        ':leave',
        [
          style({ transform: "translateX(0)", opacity: 1 }),
          animate('300ms ease-in',
            style({ transform: "translateX(-100%)", opacity: 0 }))
        ]
      )
    ]
  )
