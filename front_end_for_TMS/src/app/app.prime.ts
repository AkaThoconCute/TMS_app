import Aura from '@primeuix/themes/aura';
import { definePreset } from '@primeuix/themes';
import { PrimeNGConfigType } from 'primeng/config';

const zinc = {
  50: '{zinc.50}',
  100: '{zinc.100}',
  200: '{zinc.200}',
  300: '{zinc.300}',
  400: '{zinc.400}',
  500: '{zinc.500}',
  600: '{zinc.600}',
  700: '{zinc.700}',
  800: '{zinc.800}',
  900: '{zinc.900}',
  950: '{zinc.950}'
}

const green = {
  50: '{green.50}',
  100: '{green.100}',
  200: '{green.200}',
  300: '{green.300}',
  400: '{green.400}',
  500: '{green.500}',
  600: '{green.600}',
  700: '{green.700}',
  800: '{green.800}',
  900: '{green.900}',
  950: '{green.950}'
}

const Noir = definePreset(Aura, {
  semantic: {
    primary: zinc,
    colorScheme: {
      light: {
        primary: {
          color: '{zinc.950}',
          inverseColor: '#ffffff',
          hoverColor: '{zinc.900}',
          activeColor: '{zinc.800}'
        },
        highlight: {
          background: '{zinc.950}',
          focusBackground: '{zinc.700}',
          color: '#ffffff',
          focusColor: '#ffffff'
        }
      },
      dark: {
        primary: {
          color: '{zinc.50}',
          inverseColor: '{zinc.950}',
          hoverColor: '{zinc.100}',
          activeColor: '{zinc.200}'
        },
        highlight: {
          background: 'rgba(250, 250, 250, .16)',
          focusBackground: 'rgba(250, 250, 250, .24)',
          color: 'rgba(255,255,255,.87)',
          focusColor: 'rgba(255,255,255,.87)'
        }
      }
    }
  }
});

const Green = definePreset(Aura, {
  semantic: {
    primary: green,
  }
});

export const appPrimeNG: PrimeNGConfigType = {
  theme: {
    preset: Noir, // Aura, Noir, Green
    options: {
      cssLayer: {
        name: 'primeng',
        order: 'theme, base, primeng'
      },
    }
  },
  ripple: true
}