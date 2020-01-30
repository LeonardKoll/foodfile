import React from 'react';
import { Container } from 'reactstrap';
import { NavMenu } from './NavMenu';
import { Footer } from './Footer'

export function Layout ({mode, children})
{
  return (
    <div>
      <NavMenu mode={mode} />
      <Container>
        {children}
      </Container>
      <Footer mode={mode} />
    </div>
  );
}

/*
export class Layout extends Component {
  static displayName = Layout.name;

  render () {
    return (
      <div>
        <NavMenu />
        <Container>
          {this.props.children}
        </Container>
        <Footer />
      </div>
    );
  }
}
*/
