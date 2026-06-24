// Creator: WebInspector 537.36

import { sleep, group } from 'k6'
import http from 'k6/http'

export const options = {}

export default function main() {
  let response

  group(
    'page_5 - https://dev.manage-free-school-projects.education.gov.uk/signin-oidc',
    function () {
      response = http.post(
        'https://dev.manage-free-school-projects.education.gov.uk/signin-oidc',
        {
          id_token:
            '',
          client_info:
            '',
          state:
            '',
          session_state: '',
        },
        {
          headers: {
            accept:
              'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7',
            'accept-encoding': 'gzip, deflate, br, zstd',
            'accept-language': 'en-GB,en;q=0.9',
            'cache-control': 'max-age=0',
            'content-type': 'application/x-www-form-urlencoded',
            cookie:
              '.AspNetCore.OpenIdConnect.Nonce.; .AspNetCore.Correlation.; ASLBSA=; ASLBSACORS=',
            origin: 'https://login.microsoftonline.com',
            priority: 'u=0, i',
            referer: 'https://login.microsoftonline.com/',
            'sec-ch-ua': '"Not/A)Brand";v="8", "Chromium";v="126", "Google Chrome";v="126"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
            'sec-fetch-dest': 'document',
            'sec-fetch-mode': 'navigate',
            'sec-fetch-site': 'cross-site',
            'upgrade-insecure-requests': '1',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36',
          },
        }
      )

      response = http.get('https://dev.manage-free-school-projects.education.gov.uk/', {
        headers: {
          accept:
            'text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7',
          'accept-encoding': 'gzip, deflate, br, zstd',
          'accept-language': 'en-GB,en;q=0.9',
          'cache-control': 'max-age=0',
          cookie:
            'ASLBSA=; ASLBSACORS=; .ManageFreeSchoolProjects.Login=; .ManageFreeSchoolProjects.LoginC1=; .ManageFreeSchoolProjects.LoginC2=',
          priority: 'u=0, i',
          referer: 'https://login.microsoftonline.com/',
          'sec-ch-ua': '"Not/A)Brand";v="8", "Chromium";v="126", "Google Chrome";v="126"',
          'sec-ch-ua-mobile': '?0',
          'sec-ch-ua-platform': '"Windows"',
          'sec-fetch-dest': 'document',
          'sec-fetch-mode': 'navigate',
          'sec-fetch-site': 'cross-site',
          'upgrade-insecure-requests': '1',
          'user-agent':
            'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36',
        },
      })

      response = http.get(
        'https://dev.manage-free-school-projects.education.gov.uk/?handler=localAuthoritiesByRegion&regions=',
        {
          headers: {
            accept: '*/*',
            'accept-encoding': 'gzip, deflate, br, zstd',
            'accept-language': 'en-GB,en;q=0.9',
            cookie:
              'ASLBSA=; ASLBSACORS=; .ManageFreeSchoolProjects.Login=; .ManageFreeSchoolProjects.LoginC1=; .ManageFreeSchoolProjects.LoginC2=',
            priority: 'u=1, i',
            referer: 'https://dev.manage-free-school-projects.education.gov.uk/',
            'sec-ch-ua': '"Not/A)Brand";v="8", "Chromium";v="126", "Google Chrome";v="126"',
            'sec-ch-ua-mobile': '?0',
            'sec-ch-ua-platform': '"Windows"',
            'sec-fetch-dest': 'empty',
            'sec-fetch-mode': 'cors',
            'sec-fetch-site': 'same-origin',
            'user-agent':
              'Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/126.0.0.0 Safari/537.36',
            'x-requested-with': 'XMLHttpRequest',
          },
        }
      )
    }
  )

  // Automatically added sleep
  sleep(1)
}
