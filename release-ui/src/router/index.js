import Vue from 'vue'
import VueRouter from 'vue-router'
import Home from '../views/Home.vue'

Vue.use(VueRouter)
const routes = [
    {
        path: '/',
        name: 'Home',
        component: Home
    },
    {
        path: '/release/:iterationid',
        name: 'Release',
        props: true,
        component: () =>
            import('../views/Release.vue'),
    },
    {
        path: '/release/:iterationid/:taskid',
        name: 'ReleasePipeline',
        component: () =>
            import('../views/ReleasePipeline.vue')
    }
]
const router = new VueRouter({
    routes,
    base: process.env.BASE_URL,
    mode: "history"
})
export default router