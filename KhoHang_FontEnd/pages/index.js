import Head from 'next/head'
import Header from '@/components/Header/Header'
import Main from '@/components/Main/Main'
import Footer from '@/components/Footer/Footer'
import ScrollUp from '@/components/ScrollUp/ScrollUp'

export default function Home() {
  return (
    <div>
      <Head>
        <title>Kho hàng</title>
        <meta
          name='description'
          content='Trang web kho hàng'
        />
        <link rel='icon' href='./images/favicon.png' />
        <link
          href='https://unpkg.com/boxicons@2.0.9/css/boxicons.min.css'
          rel='stylesheet'
        />
      </Head>
      <Header />
      <Main />
      <Footer />
      <ScrollUp />
    </div>
  )
}
